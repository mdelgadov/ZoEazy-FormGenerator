using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Windows.Input;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Toolkit.NumericEntry;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using ZoEazy.Converters;
using ZoEazy.Models;
using ZoEazy.Resources;
using ContentView = Microsoft.Maui.Controls.ContentView;

#if ANDROID
using ViewCompat = AndroidX.Core.View.ViewCompat;
#endif

namespace ZoEazy.Extensions;

public static class ViewExtensions
{
    // ReSharper disable once TooManyDeclarations
    // all of them needed
    // ReSharper disable once CognitiveComplexity
    // legacy from our friends from Uranium
    public static T? FindInChildrenHierarchy<T>(this View view,
        Func<T, bool>? expression = null) where T : VisualElement
    {
        while (true)
        {
            expression ??= _ => true;

            switch (view)
            {
                case Layout layout:
                {
                    foreach (var item in layout.Children)
                    {
                        switch (item)
                        {
                            case T found when expression(found):
                                return found;
                            case Layout anotherLayout:
                                return anotherLayout.FindInChildrenHierarchy(expression);
                        }
                    }

                    break;
                }
                case ContentView contentView:
                    view = contentView.Content;
                    continue;
            }

            return null;
        }
    }

    // ReSharper disable once CognitiveComplexity
    // it's a complex method, but it's not too complex
    public static IEnumerable<T> FindManyInChildrenHierarchy<T>(this View view, Func<T, bool>? expression = null)
        where T : View
    {
        expression ??= _ => true;

        if (view is T itself && expression(itself))
        {
            yield return itself;
        }

        switch (view)
        {
            case IContentView { Content: View contentViewContent }:
            {
                foreach (var child in contentViewContent.FindManyInChildrenHierarchy(expression))
                {
                    yield return child;
                }

                break;
            }

            case Layout layout:
            {
                foreach (var item in layout.Children)
                {
                    if (item is T found && expression(found))
                    {
                        yield return found;
                    }

                    if (item is not View childView) continue;
                    foreach (var child in childView.FindManyInChildrenHierarchy(expression))
                    {
                        yield return child;
                    }
                }

                break;
            }
            
        }
    }

    public static void InsertKeyboard(this Entry entry, PropertyInfo property)
    {
        entry.Keyboard = GetKeyboard();

        Keyboard GetKeyboard()
        {
            var prop = property.GetCustomAttributes<KeyboardAttribute>().FirstOrDefault();
            if (prop is null)
            {
                return Keyboard.Default;
            }
            return prop.KeyboardType switch
            {
                KeyboardType.Plain => Keyboard.Plain,
                KeyboardType.Email => Keyboard.Email,
                KeyboardType.Text => Keyboard.Text,
                KeyboardType.Url => Keyboard.Url,
                KeyboardType.Numeric => Keyboard.Numeric,
                KeyboardType.Telephone => Keyboard.Telephone,
                KeyboardType.Chat => Keyboard.Chat,
                KeyboardType.Date => Keyboard.Date,
                KeyboardType.Time => Keyboard.Time,
                KeyboardType.Password => Keyboard.Password,
                _ => Keyboard.Default
            };
        }
    }
    public static void InsertMasks(this Entry entry, PropertyInfo property)
    {
        if (IsPhone())
        {
            entry.Keyboard = Keyboard.Numeric;
            entry.Behaviors.Add(new MaskedBehavior()
            {
                Mask = "(XXX) XXX-XXXX"
            });
        }
        if (IsDate())
        {
            entry.Keyboard = Keyboard.Numeric;
            entry.Behaviors.Add(new MaskedBehavior()
            {
                Mask = "XX/XX/XXXX"
            });
        }
        return;
        bool IsPhone() => property.GetCustomAttributes<PhoneAttribute>().Any();
        bool IsDate() => property.GetCustomAttributes<DateOnlyValidationAttribute>().Any();
    }
    public static void InsertHint(this SfTextInputLayout editor, PropertyInfo property, bool hintAlwaysFloated = false)
    {

        editor.IsHintAlwaysFloated = hintAlwaysFloated;
        
        var attribute = property.GetCustomAttribute<DisplayAttribute>();
        if (attribute is not null)
        {
            editor.Hint = attribute.GetName()!;
            return; 
        }
        var name = Localize.Get(property.Name);
        editor.Hint = name ?? property.Name;
    }
    public static void InsertHelper(this SfTextInputLayout editor,
        PropertyInfo property,
        HelpDictionary helpMessages)
    {
        
        editor.HelperText = property.GetHelper(helpMessages);
    }
    public static void InsertPlaceholder(this Entry entry, PropertyInfo property)
    {
        if (IsPhone())
        {
            entry.Placeholder = "999 999 9999";
        }
        if (IsDate())
        {
            entry.Placeholder = "MM dd yyyy";
        }
        return;
        bool IsPhone() => property.GetCustomAttributes<PhoneAttribute>().Any();
        bool IsDate() => property.GetCustomAttributes<DateOnlyValidationAttribute>().Any();
    }
    public static void InsertPassword(this SfTextInputLayout editor, PropertyInfo property)
    {
        if (IsPassword()) editor.EnablePasswordVisibilityToggle = true;

        bool IsPassword() => property.GetCustomAttributes<KeyboardAttribute>()
            .Any(k => k.KeyboardType == KeyboardType.Password);
    }
    public static void InsertRequired(this SfTextInputLayout editor, PropertyInfo property)
    {
        if (IsNotRequired()) return;
        var image = StandardImages.Required(IsPassword());

        editor.Children.Add(image);

        bool IsNotRequired() =>
            !property.GetCustomAttributes<RequiredAttribute>().Any() &&
            !property.GetCustomAttributes<MustBeTrueAttribute>().Any();

        bool IsPassword() => property.GetCustomAttributes<KeyboardAttribute>()
            .Any(k => k.KeyboardType == KeyboardType.Password);
    }
  
    public static void SetStyleId(this Entry entry, string property)
    {
        entry.StyleId = property;
    }
    public static void InsertBehaviorCheckedChanged(this RadioButton radioButton, ICommand unfocusedCommand)
    {
        radioButton.Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(radioButton.CheckedChanged),
            Command = unfocusedCommand,
            EventArgsConverter = new CheckboxArgsConverter(radioButton.GetId())
        });
    }

    public static void InsertBehaviorCheckedChanged(this CheckBox checkBox, ICommand unfocusedCommand)
    {
        checkBox.Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(checkBox.CheckedChanged),
            Command = unfocusedCommand,
            EventArgsConverter = new CheckboxArgsConverter(checkBox.GetId())
        });
    }
    public static void InsertBehaviorDateSelected(this DatePicker view, ICommand dateSelectedCommand)
    {

        view.Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(DatePicker.DateSelected),
            Command = dateSelectedCommand,
            EventArgsConverter = new DateSelectedArgsConverter(view.GetId())
        });
    }

    public static void InsertBehaviorTapped(this View view, ICommand unfocusedCommand, string propertyName)
    {
        var tapGestureRecognizer = new TapGestureRecognizer();

        tapGestureRecognizer.Tapped += (_, _) =>
        {
            if (unfocusedCommand.CanExecute(null))
            {
                unfocusedCommand.Execute(null);
            }
        };
        view.GestureRecognizers.Add(tapGestureRecognizer);
    }

    public static void InsertBehaviorTimeSelected(this TimePicker timePicker, ICommand command)
    {
        timePicker.Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(timePicker.TimeSelected),
            Command = command,
            EventArgsConverter = new TimeSelectedArgsConverter(timePicker.GetId())
        });
    }
    public static void InsertBehaviorUnfocused(this View view, ICommand unfocusedCommand)
    {
        view.Behaviors.Add(new EventToCommandBehavior
        {
            EventName = nameof(view.Unfocused),
            Command = unfocusedCommand,
            EventArgsConverter = new FocusedArgsConverter()
        });
    }
   
    // ReSharper disable once TooManyArguments
    // all of them needed
    public static void InsertValidator(this SfTextInputLayout editor,
        PropertyInfo property,
        PropertyInfo[] errorProperties,
        ObservableObject messages)
    {
        var message = errorProperties.FirstOrDefault(p => p.Name == property.Name);
        if (message is null) return;
        editor.SetBinding(SfTextInputLayout.ErrorTextProperty, new Binding(message.Name, source: messages));
        editor.SetBinding(SfTextInputLayout.HasErrorProperty, new Binding(message.Name, source: messages, converter: new TextToBoolConverter()));
    }
    public static void Configure(this SfNumericEntry entry, PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<NumericAttribute>();
        if (attribute is null) return;

        InsertFormat(attribute, entry);
        InsertMaximum(attribute, entry);
        InsertMinimum(attribute, entry);
        InsertPlaceholderAndFormat(entry, attribute);

        entry.AllowNull = (property.PropertyType.IsNullable());
        entry.IsEditable = attribute.IsEditable;
    }

    private static void InsertPlaceholderAndFormat(SfNumericEntry entry, NumericAttribute attribute)
    {
        entry.Placeholder = attribute.Format;

        if (attribute.CustomFormat == CustomFormat.None)
            entry.CustomFormat = entry.Placeholder;

        if(attribute is { CustomFormat: CustomFormat.P, PercentageMode: "Compute" })
        {
            entry.PercentDisplayMode = PercentDisplayMode.Compute;
        }

        if (attribute.MaxDecimal is not null)
        {
            entry.MaximumNumberDecimalDigits = Convert.ToInt16(attribute.MaxDecimal);
        }
    }

    private static void InsertMinimum(NumericAttribute attribute, SfNumericEntry entry)
    {
        if (attribute.Minimum is not null)
        {
            entry.Minimum = double.Parse(attribute.Minimum);
        }
    }

    private static void InsertMaximum(NumericAttribute attribute, SfNumericEntry entry)
    {
        if (attribute.Maximum is not null)
        {
            entry.Maximum = double.Parse(attribute.Maximum);
        }
    }

    private static void InsertFormat(NumericAttribute attribute, SfNumericEntry entry)
    {
        if (attribute.CustomFormat != CustomFormat.None)
        {
            entry.CustomFormat = "C2";

        } else if (attribute.SpecialFormat is not null)
        {
            entry.CustomFormat = attribute.SpecialFormat;
        }
    }
}

public static class ResourceDictionaryExtensions
{
    public static Color? GetColor(this ResourceDictionary dictionary, string hexColor)
    {
        if (dictionary.TryGetValue(hexColor, out var obj))
        {
            if (obj is Color color)
                return color;
            return null;
        }


        throw new KeyNotFoundException($"The color '{hexColor}' was not found in the resource dictionary.");
    }
}

public static class PropertyInfoExtensions
{
    public static bool IsForPicker(this PropertyInfo property) => property.GetCustomAttributes<PickerAttribute>().Any();

    public static bool IsForRadio(this PropertyInfo property) => property.GetCustomAttributes<RadioAttribute>().Any();

    public static bool IsForBars(this PropertyInfo property) => property.GetCustomAttributes<BarsAttribute>().Any();

    public static bool IsForEditor(this PropertyInfo property) => property.GetCustomAttributes<EditorAttribute>().Any();

    public static string GetHelper(this PropertyInfo property, HelpDictionary helpMessages)
    {
        return helpMessages.HelpMessages().TryGetValue(property.Name, out var value) ? value : string.Empty;
    }
}
