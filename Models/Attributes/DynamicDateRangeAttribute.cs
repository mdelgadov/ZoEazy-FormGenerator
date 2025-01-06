using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DynamicDateRangeAttribute(int minusYear, int plusYear) : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateOnly && value is not DateTime)
            return ValidationResult.Success!;

        if (value is not DateOnly date)
            date = DateOnly.FromDateTime((DateTime)value);
        


        var minus = DateOnly.FromDateTime(DateTime.Now.AddYears(-minusYear));
        var plus = DateOnly.FromDateTime(DateTime.Now.AddYears(plusYear));
            
        if (date < minus || date > plus)
        {
            return new ValidationResult(GetErrorMessage());
        }

        return ValidationResult.Success!;
    }


    private string GetErrorMessage()
    {
        if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
        {
            var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
            var errorMessage = resourceManager.GetString(ErrorMessageResourceName);
            if (errorMessage != null) return string.Format(errorMessage, minusYear, plusYear);
        }
        return $"Date must be between {minusYear} and {plusYear}.";
    }
}