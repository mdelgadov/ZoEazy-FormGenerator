using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DateOnlyValidationAttribute(DateValidationType dateValidationType = DateValidationType.Date,DateFormatType dateFormatType = DateFormatType.MMSDDSYYYY)
    : ValidationAttribute
{
    public DateValidationType DateValidationType { get; } = dateValidationType;
    public DateFormatType DateFormatType { get; } = dateFormatType;

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly && value is not string)
            return ValidationResult.Success!;

        var format = GetFormat();

        var dateString = value switch
        {
            DateOnly date => date.ToString(format),
            _ => value as string
        };

        var result = DateTime.TryParseExact(dateString,
            format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _)
            ? ValidationResult.Success!
            : new ValidationResult(GetErrorMessage());
        return result;

        string GetFormat ()
        {
            var f = DateFormatType.ToString();
            f = f.Replace('Y', 'y')
                .Replace('D', 'd')
                .Replace('S', '/')
                .Replace('A', '-');
            return f;
        }

    }
    
    private string GetErrorMessage()
    {
        if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
        {
            var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
            var errorMessage = resourceManager.GetString(ErrorMessageResourceName);
            if (errorMessage != null) return string.Format(errorMessage);
        }
        return $"Invalid Date.";
    }
}