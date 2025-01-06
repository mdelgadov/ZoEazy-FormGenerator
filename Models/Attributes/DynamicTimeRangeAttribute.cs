using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

public class DynamicTimeRangeAttribute(int hourInit, int hourEnd) : ValidationAttribute
{
    private  const string InvalidType = "InvalidType";
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not TimeOnly time)
            return new ValidationResult( GetErrorMessage());

        var init = TimeOnly.MinValue.AddHours(hourInit);
        var end= TimeOnly.MinValue.AddHours(hourEnd);

        if (time >= init && time <= end) return ValidationResult.Success!;

        return new ValidationResult(GetErrorMessage());
    }

    private string GetErrorMessage()
    {
        if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
        {
            var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
            var errorMessage = resourceManager.GetString(ErrorMessageResourceName);
            if (errorMessage != null) return string.Format(errorMessage, hourInit, hourEnd);
        }
        return $"Date must be between {hourInit} and {hourEnd}.";
    }

    
}