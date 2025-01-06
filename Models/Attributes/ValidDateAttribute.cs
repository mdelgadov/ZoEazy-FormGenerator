using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ValidDateAttribute(string format = "MMMM/dd/yyyy") : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly && value is not DateTime && value is not string)
            return ValidationResult.Success!;

        var dateString = value switch
        {
            DateOnly date => date.ToString(),
            DateTime dateTime => (DateOnly.FromDateTime(dateTime)).ToString(),
            _ => value as string
        };

        return DateTime.TryParseExact(dateString,
            format,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _)
            ? ValidationResult.Success!
            : new ValidationResult(GetErrorMessage());
    }


    private string GetErrorMessage()
    {
        if (ErrorMessageResourceType == null || string.IsNullOrEmpty(ErrorMessageResourceName)) return $"Invalid Date.";

        var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
        var errorMessage = resourceManager.GetString(ErrorMessageResourceName);
        
        return errorMessage != null ? string.Format(errorMessage) : $"Invalid Date.";
    }
}