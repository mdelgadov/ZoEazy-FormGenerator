using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.NumericEntry;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Input;
using ZoEazy.Converters;
using ZoEazy.Extensions;
using ZoEazy.Models;

// ReSharper disable MethodTooLong
// several methods are long but chop them out for the sake of it would only make it more convoluted... 
// so, between two evils...

namespace ZoEazy.Controls;

public class AutoFormView : VerticalStackLayout
{
    public ObservableValidator? Source { get; set; }
    public ObservableObject? Messages { get; set; }
    public PropertyInfo[]? EditingProperties { get; private set; }
    public PropertyInfo[]? ErrorProperties { get; set; }
    public HelpDictionary? HelpMessages { get; set; }
    public Type HierarchyLimitType { get; set; } = typeof(object);

    public static readonly BindableProperty ParametersProperty = BindableProperty.Create(
    nameof(Parameters),
    typeof(Inputs),
    typeof(AutoFormView),
    propertyChanged: (bindable, _, _) => ((bindable as AutoFormView)!).OnParametersChanged());
    public Inputs Parameters { get => (Inputs)GetValue(ParametersProperty); set => SetValue(ParametersProperty, value); }

    public ICommand UnfocusedCommand
    {
        get => (ICommand)GetValue(UnfocusedCommandProperty);
        init => SetValue(UnfocusedCommandProperty, value);
    }

    public static readonly BindableProperty UnfocusedCommandProperty = BindableProperty.Create(nameof(UnfocusedCommand),
        typeof(ICommand),
        typeof(AutoFormView));
    public ICommand SelectedIndexCommand
    {
        get => (ICommand)GetValue(SelectedIndexCommandProperty);
        set => SetValue(SelectedIndexCommandProperty, value);
    }

    private static readonly BindableProperty SelectedIndexCommandProperty = BindableProperty.Create(nameof(SelectedIndexCommand),
        typeof(ICommand),
        typeof(AutoFormView));

    public ICommand DateSelectedCommand
    {
        get => (ICommand)GetValue(DateSelectedCommandProperty);
        set => SetValue(DateSelectedCommandProperty, value);
    }

    public static readonly BindableProperty DateSelectedCommandProperty = BindableProperty.Create(nameof(DateSelectedCommand),
        typeof(ICommand),
        typeof(AutoFormView));

    public ICommand TimeSelectedCommand
    {
        get => (ICommand)GetValue(TimeSelectedCommandProperty);
        set => SetValue(TimeSelectedCommandProperty, value);
    }

    public static readonly BindableProperty TimeSelectedCommandProperty = BindableProperty.Create(nameof(TimeSelectedCommand),
        typeof(ICommand),
        typeof(AutoFormView));

    public ICommand CheckedChangedCommand
    {
        get => (ICommand)GetValue(CheckedChangedCommandProperty);
        set => SetValue(CheckedChangedCommandProperty, value);
    }

    public static readonly BindableProperty CheckedChangedCommandProperty = BindableProperty.Create(nameof(CheckedChangedCommand),
        typeof(ICommand),
        typeof(AutoFormView));

    private Layout[] _externalLayouts = [];

    private Layout _itemsLayout = new VerticalStackLayout
    {
        Spacing = 20,
        Padding = 10,
    };

    public Layout ItemsLayout
    {
        get => _itemsLayout;
        set
        {
            Children.Remove(_itemsLayout);
            while (_itemsLayout.Children.Count > 0)
            {
                var item = _itemsLayout.Children.First();
                _itemsLayout.Children.Remove(item);
                value.Children.Add(item);
            }
            _itemsLayout = value;
            Children.Add(_itemsLayout);
        }
    }

   

    public AutoFormView()
    {
        Children.Add(_itemsLayout);
    }

    private void OnParametersChanged()
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
        Source = Parameters.FormContent.Fields;
        Messages = Parameters.FormContent.Messages;
        HelpMessages = Parameters.FormContent.HelpMessages;
        _externalLayouts = Parameters.ExternalLayouts;

        EditingProperties = Source!.GetType()
            .GetProperties(flags)
            .Where(x => HierarchyLimitType.IsAssignableFrom(x.PropertyType))
            .ToArray();

        ErrorProperties = Messages
            .GetType()
            .GetProperties(flags)
            .ToArray();

        Render();
    }
    private void Render()
    {
        if (Source is null)
        {
            _itemsLayout.Children.Clear();
            return;
        }

        ParseProperties();
    }
    private void ParseProperties()
    {
        for (var index = 0; index < EditingProperties!.Length; index++)
        {
            var property = EditingProperties[index];

            InsertPoint(property);

            if (InternalProperty(property)) continue;

            var group = IsGrouped(property);

            if (!group.isInAGroup)
                InsertProperty(property);
            else
                index = InsertGroup(index, (int)group.group!);
        }

        static bool InternalProperty(PropertyInfo property)
        {
            return property.Name == "HasErrors"
                   || property.Name == "HasNoErrors"
                   || property.GetCustomAttributes<HiddenAttribute>().FirstOrDefault() is not null;
        }
    }
    private static (bool isInAGroup, double? group) IsGrouped(PropertyInfo property)
    {
        var attr = App.IsPhone
            ? property.GetCustomAttributes<PhoneGroupAttribute>().FirstOrDefault()
            : (GroupAttribute?)property.GetCustomAttributes<DesktopGroupAttribute>().FirstOrDefault();

        return (attr is null) ? (false, null) : (true, group: attr.Group);
    }
    private void InsertPoint(PropertyInfo propertyInfo)
    {
        var attr = propertyInfo.GetCustomAttributes<InsertPointAttribute>().FirstOrDefault();

        if (attr is null) return;

        _itemsLayout.Children.Add(_externalLayouts[attr.Point]);
    }
    private void InsertProperty(PropertyInfo property)
    {
        _itemsLayout.Add(GetEditor(property));
    }
    private int InsertGroup(int index, int group)
    {
        var properties = App.IsPhone
            ? EditingProperties!.Where(prop => prop.GetCustomAttribute<PhoneGroupAttribute>()?.Group == group).ToArray()
            : EditingProperties!.Where(prop => prop.GetCustomAttribute<DesktopGroupAttribute>()?.Group == group).ToArray();

        var grid = new Grid
        {
            ColumnSpacing = 20,
            VerticalOptions = LayoutOptions.Start,
            RowDefinitions = Rows.Define(Auto),
        };
        foreach (var property in properties)
        {

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

            var editor = GetEditor(property);
            editor.Column(grid.ColumnDefinitions.Count - 1);
            grid.Add(editor);
        }
        _itemsLayout.Add(grid);
        return index + properties.Length - 1;
    }
    private View GetEditor(PropertyInfo property)
    {

        // special case, a picker can return any kind of value to the bound property
        if (property.IsForPicker()) return EditorForPicker(property);
        if (property.IsForEditor()) return EditorForEditor(property);

        var type = property.PropertyType.AsNonNullable();

        return type switch
        {
            { IsEnum: true } => EditorForEnum(property),
            not null when type == typeof(DateTime) => EditorForDateTime(property), // datetime binds to datepicker but doesn't to sting
            not null when type == typeof(DateOnly) => EditorForDateOnly(property), // vice versa, DateOnly binds to string but doesn't to datepicker...
            not null when type == typeof(TimeOnly) => EditorForTimeOnly(property),
            not null when type == typeof(Keyboard) => EditorForKeyboard(property),
            not null when type == typeof(bool) => EditorForBoolean(property),
            not null when type == typeof(string) => EditorForString(property),
            not null when type == typeof(int) ||
                          type == typeof(decimal) ||
                          type == typeof(double) ||
                          type == typeof(float) => EditorForNumeric(property),

            _ => EditorInvalid(property)
        };
    }
    private SfTextInputLayout EditorForPicker(PropertyInfo property)
    {
        var entry = GetPicker();

        return GetWrapper(property, entry);

        Picker GetPicker()
        {
            var picker = new Picker
            {
                Title = null!, // important to avoid clash with hint in the SfTextInputLayout
                WidthRequest = App.IsPhone ? 300 : 500,
                ItemsSource = GetSource(),
            };
            picker.SetBinding(Picker.SelectedItemProperty, new Binding(property.Name, source: Source));
            return picker;

            List<string> GetSource()
            {
                var attr = property.GetCustomAttribute<PickerAttribute>();
                if (attr is null) return new List<string>();
                var method = "get_" + attr.Method;

                var methodInfo = Source!.GetType().GetMethod(method, BindingFlags.Static | BindingFlags.Public);
                var result = methodInfo?.Invoke(Source, null) as List<string>;
                return result ?? new List<string>();
            }
        }
    }

    private SfTextInputLayout EditorForEditor(PropertyInfo property)
    {
        var entry = GetEntryEditor();

        return GetWrapper(property, entry, true);

        Editor GetEntryEditor()
        {
            var attribute = property.GetCustomAttributes<EditorAttribute>().First();
            return new Editor
            {
                IsReadOnly = attribute.ReadOnly,
                Text = GetSource(),
                HeightRequest = attribute.Height,
                MaximumHeightRequest = attribute.MaxHeight,
                AutoSize = attribute.AutoSize,
                IsSpellCheckEnabled = attribute.IsSpellCheckEnabled,
                IsTextPredictionEnabled = attribute.IsTextPredictionEnabled,
            };


            string GetSource()
            {
                var attr = property.GetCustomAttribute<EditorAttribute>();
                if (attr is null) return string.Empty;
                var method = "get_" + attr.SourceMethod;

                var methodInfo = Source!.GetType().GetMethod(method, BindingFlags.Static | BindingFlags.Public);
                var result = methodInfo?.Invoke(Source, null) as string;
                return result ?? string.Empty;
            }

        }
    }

    private SfTextInputLayout EditorForString(PropertyInfo property)
    {
        var entry = GetEntryForString();

        var wrapper = GetWrapper(property, entry);

        wrapper.InsertPassword(property);

        return wrapper;

        Entry GetEntryForString()
        {
            var e = new Entry();
            e.InsertId(property.Name, nameof(Entry));
            e.SetBinding(Entry.TextProperty, new Binding(property.Name, source: Source));
            e.InsertBehaviorUnfocused(UnfocusedCommand);
            e.InsertMasks(property);
            e.InsertKeyboard(property);
            e.InsertPlaceholder(property);

            return e;
        }
    }

    private SfTextInputLayout EditorForNumeric(PropertyInfo property)
    {
        var numeric = new SfNumericEntry
        {
            HorizontalTextAlignment = TextAlignment.Start,
            VerticalTextAlignment = TextAlignment.Center,
            ShowBorder = true,
            ShowClearButton = true,
            Minimum = 0,
            // todo: there is an apparent bug, when the input is too big, it hangs, the whole app... keep a lid to avoid hang
            Maximum = 10000000
        };

        numeric.Configure(property);
        numeric.InsertId(property.Name, nameof(SfNumericEntry));
        numeric.SetBinding(SfNumericEntry.ValueProperty,
            new Binding(property.Name, source: Source, converter: new NumericToDoubleConverter(), converterParameter: property));

        return GetWrapper(property, numeric);

    }

    private Grid EditorForBoolean(PropertyInfo property)
    {
        var entry = GetEntry();

        var editor = CreateWrapper();
        return new Grid
        {
            ColumnDefinitions = Columns.Define(GridLength.Auto, GridLength.Star),
            RowDefinitions = Rows.Define(GridLength.Auto),
            Margin = Thickness.Zero,
            Padding = Thickness.Zero,
            Children =
            {
                editor
            }
        };

        CheckBox GetEntry()
        {
            var e = new CheckBox
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
            };

            e.InsertId(property.Name, nameof(CheckBox));

            e.SetBinding(CheckBox.IsCheckedProperty, new Binding(property.Name, source: Source));
            e.InsertBehaviorUnfocused(UnfocusedCommand);
            e.InsertBehaviorCheckedChanged(CheckedChangedCommand);
            return e;
        }

        SfTextInputLayout CreateWrapper()
        {
            var label = new Label
            {
                Text = property.GetHelper(HelpMessages!),
                Padding = Thickness.Zero,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                MinimumWidthRequest = 200,

            };

            var container = new Grid
            {
                Padding = Thickness.Zero,
                RowDefinitions = Rows.Define(GridLength.Auto),
                ColumnDefinitions = Columns.Define(30, GridLength.Star),
                Children =
                {
                    entry,
                    label.Column(1),
                },
            };

            return GetWrapper(property, container, hintAlwaysFloated: true, skipHelper: true);
        }
    }

    private SfTextInputLayout EditorForEnum(PropertyInfo property)
    {
        var layout = GetLayout();

        var wrapper = GetWrapper(property, layout, true);
        wrapper.InsertValidator(property, ErrorProperties!, Messages!);

        return wrapper;

        FlexLayout GetLayout()
        {
            var flexLayout = new FlexLayout
            {
                Direction = FlexDirection.Row,
                JustifyContent = FlexJustify.SpaceEvenly,
                AlignItems = FlexAlignItems.Center,
                Wrap = FlexWrap.Wrap
            };

            var values = Enum.GetValues(property.PropertyType.AsNonNullable());

            foreach (var value in values)
            {
                var type = value.GetType();
                var field = type.GetField(value.ToString()!);
                var displayAttribute = field!.GetCustomAttribute<DisplayAttribute>();

                var radio = new RadioButton
                {
                    Content = displayAttribute?.GetName() ?? value.ToString(),
                    Value = value
                };
                radio.SetBinding(RadioButton.IsCheckedProperty,
                    new Binding(property.Name, BindingMode.OneWay,
                        source: Source,
                        converter: new EnumToBoolConverter(),
                        converterParameter: value));

                flexLayout.Children.Add(radio);
            }
            return flexLayout;
        }
    }
    private SfTextInputLayout EditorForKeyboard(PropertyInfo property)
    {
        var picker = new Picker
        {
            ItemsSource = typeof(Keyboard)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(x => x.GetValue(null))
                .ToArray()
        };

        picker.SetBinding(Picker.SelectedItemProperty, new Binding(property.Name, source: Source));
        picker.Title = property.Name;

        return GetWrapper(property, picker);
    }
    private SfTextInputLayout EditorForDateTime(PropertyInfo property)
    {
        var view = GetDatePicker();
        return GetWrapper(property, view, true);

        DatePicker GetDatePicker()
        {
            var datePicker = new DatePicker
            {
                Format = "d"
            };

            datePicker.SetBinding(DatePicker.DateProperty, new Binding(property.Name, BindingMode.TwoWay, source: Source));
            datePicker.InsertId(property.Name, nameof(DatePicker));
            datePicker.InsertBehaviorDateSelected(DateSelectedCommand);
            return datePicker;
        }
    }

    private SfTextInputLayout EditorForDateOnly(PropertyInfo property)
    {
        var entry = GetDateNumericEntry();
        return GetWrapper(property, entry);

        Entry GetDateNumericEntry()
        {
            var e = new Entry
            {
                Keyboard = Keyboard.Numeric
            };

            e.InsertPlaceholder(property);

            e.InsertMasks(property);
            e.InsertBehaviorUnfocused(UnfocusedCommand);

            e.InsertId(property.Name, nameof(Entry));

            e.SetBinding(Entry.TextProperty,
                new Binding(property.Name,
                    BindingMode.TwoWay,
                    source: Source,
                    converter: new DateToStringConverter
                    {
                        DateFormat = "MM/dd/yyyy"
                    }));
            return e;
        }
    }
    private SfTextInputLayout EditorForTimeOnly(PropertyInfo property)
    {
        var entry = new TimePicker
        {
            Format = "H_mm",
            HorizontalOptions = LayoutOptions.Start,
            WidthRequest = App.IsPhone ? 200 : 300
        };
        entry.InsertId(property.Name, nameof(TimePicker));
        entry.InsertBehaviorTimeSelected(TimeSelectedCommand);
        entry.SetBinding(TimePicker.TimeProperty, new Binding(property.Name, source: Source, converter: new TimeOnlyToTimeSpanConverter()));

        var editor = new Grid
        {
            ColumnDefinitions = Columns.Define(GridLength.Auto, GridLength.Star),
            Children = { entry, },
        };

        var wrapper = new SfTextInputLayout
        {
            Content = editor,
        };

        wrapper.InsertHint(property, true);
        wrapper.InsertHelper(property, HelpMessages!);
        wrapper.InsertId(property.Name, nameof(SfTextInputLayout));
        wrapper.InsertRequired(property);
        wrapper.InsertValidator(property, ErrorProperties!, Messages!);
        
        var message = ErrorProperties!.FirstOrDefault(p => p.Name == property.Name);
        if(message is not null) editor.SetBinding(SfTextInputLayout.ErrorTextProperty, new Binding(message.Name, source: Messages));

        return wrapper;
    }
    private View EditorInvalid(PropertyInfo property)
    {
        return new Label
        {
            Text = $"No editor for {property.Name} ({property.PropertyType})",
            FontAttributes = FontAttributes.Italic
        };
    }

    // ReSharper disable once TooManyArguments
    // all of them functional, all of them useful... lashes?
    private SfTextInputLayout GetWrapper(PropertyInfo property,
        View entry,
        bool hintAlwaysFloated = false,
        // ReSharper disable once FlagArgument
        // a flag is a flag is a flag
        bool skipHelper = false)
    {
        var wrapper = new SfTextInputLayout
        {
            Content = entry,
        };

        wrapper.InsertHint(property, hintAlwaysFloated);

        if(!skipHelper) wrapper.InsertHelper(property, HelpMessages!);

        wrapper.InsertId(property.Name, nameof(SfTextInputLayout));
        wrapper.InsertRequired(property);
        wrapper.InsertValidator(property, ErrorProperties!, Messages!);

        return wrapper;
    }

}
