using System.Globalization;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using ZoEazy.Extensions;
using ZoEazy.Models;
using Inputs = ZoEazy.PageModels.Inputs;

namespace ZoEazy.ViewModels;
public partial class ProfileVM : BaseVM
{
    [ObservableProperty] private Inputs _inputs;
    [ObservableProperty] private FormContent _profileContent;

    private bool _hasBeenTouched;

    [ObservableProperty] private ButtonBarData[] _buttonBar;

    
    public ProfileVM(IConfiguration config) : base(config)
    {

      
        _profileContent = new FormContent(new ProfileFields(), new ProfileMessages(), new ProfileHelpDictionary());

        _inputs = new Inputs(_profileContent);

        _buttonBar = [new ButtonBarData(ResourceForm.Ok, true)];
    }


    [RelayCommand]
    private void PickPhoto(EventArgs eventArgs)
    {

    }

    [RelayCommand]
    private void CapturePhoto(EventArgs eventArgs)
    {

    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    // usually, this method would do something, left here for completeness...
    private async Task SubmitAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        //todo: to illustrate the flow in a real case scenario
    }

    private bool CanSubmit()
    {
        return ProfileContent.Fields.HasNoErrors && _hasBeenTouched;
    }
    [RelayCommand]
    private void TimeSelected(VvTimeChangedEventArgs args)
    {
        var propertyName = GetFieldName(args.ElementName);

        var property = ProfileContent.Fields.GetType().GetProperty(propertyName)!;
        var newTime= args.NewTime;

        var time = TimeOnly.FromTimeSpan(newTime);

        CommonCase(property, time);

        ProfileContent.Fields.ValidateAllProperties();
        RefreshCanSubmit();

    }


    [RelayCommand]
    private void DateSelected(VvDateSelectedEventArgs args)
    {
        var propertyName = GetFieldName(args.ElementName);

        var property = ProfileContent.Fields.GetType().GetProperty(propertyName)!;
        var newDate = args.NewDate;

        object date = (IsDateOnly(property)) ? DateOnly.FromDateTime(newDate) : newDate;

        CommonCase(property, date);

        ProfileContent.Fields.ValidateAllProperties();
        RefreshCanSubmit();
    }

    [RelayCommand]
    private void Unfocused(EventArgs eventArgs)
    {
        var visualElement = ((FocusEventArgs)eventArgs).VisualElement;

        var id = visualElement.GetId();
        var name = GetFieldName(id);
        var property = ProfileContent.Fields.GetType().GetProperty(name)!;

        object value;

        if (id.EndsWith(nameof(Entry)))
           value = ((Entry)visualElement).Text;
        else if (id.EndsWith(nameof(Picker)))
            value = ((Picker)visualElement).Title;
        else if(id.EndsWith(nameof(TimePicker)))
            value = ((TimePicker)visualElement).Time;
        else
            value = ((CheckBox)visualElement).IsChecked;
        
        ValidateProperty(property, value);
    }

    private void ValidateProperty(PropertyInfo property, object? value)
    {
        _hasBeenTouched = true;

        if (IsStringDate())
            StringDateCase();
        else
            CommonCase(property, value);

        ProfileContent.Fields.ValidateAllProperties();
        RefreshCanSubmit();
        return;

        bool IsStringDate()
        {
            return property
                  .PropertyType.AsNonNullable() == typeof(DateTime) ||
                  property
                      .PropertyType.AsNonNullable() == typeof(DateOnly);
            
            //this means it is a date prop with string presentation or is null with a date prop that not accept nulls (by implication)
            // return value is string text || (string.IsNullOrEmpty(value as string) && !IsNullable(property));
        }

        void StringDateCase()
        {
            var text = value as string;
            //Special case: It is a special case if:

            // some cases need special treatment:
            // 1. Date validation
            // When using entry instead of picker, there is a special case of invalid date like Feb. 30th, etc.
            // needs first to be tested for valid and then, it can be converted to date and go with the rest of the validation

            if (DateTime
                .TryParseExact(text,
                    "MM/dd/yyyy",
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.None,
                    out var date))

            {
                value = IsDateOnly(property) ? DateOnly.FromDateTime(date) : date;
                CommonCase(property, value);
            }
            else if (string.IsNullOrEmpty(text) && IsNullable())
            {
                CommonCase(property, null);
            }
            else
                ProcessInvalidDate();

            return;

            void ProcessInvalidDate()
            {
                // sets the error message for the date field and validates the rest of the screen,
                // this in particular stops being validated. No need to further on an invalid date.
                ProfileContent.Messages.GetType().GetProperty(property.Name)?.SetValue(ProfileContent.Messages, ResourceForm.InvalidDate);
            }
        }

        bool IsNullable()
        {
            return property.PropertyType.IsNullable();
        }
    }

    private static bool IsDateOnly(PropertyInfo property)
    {
        return property.PropertyType.AsNonNullable() == typeof(DateOnly);
    }

    private string GetFieldName(string prop)
    {
        var index = prop.IndexOf('-');
        return index == -1 ? prop : prop[..index];
    }

    private void CommonCase(PropertyInfo property, object? value)
    {
        const string tail = ", ";
        var message = string.Empty;

        ProfileContent.Fields.ValidateProperty(value, property.Name);
        var errors = ProfileContent.Fields.GetErrors(property.Name).ToList();

        if (errors.Any())
        {
            errors.ForEach(f => message += f.ErrorMessage + tail);
            message = message[..^2];
        }

        ProfileContent.Messages.GetType().GetProperty(property.Name)?.SetValue(ProfileContent.Messages, message);
    }
     

    private void RefreshCanSubmit() => SubmitCommand.NotifyCanExecuteChanged();
}