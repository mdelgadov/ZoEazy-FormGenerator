
using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class EqualToPropertyAttribute : ValidationAttribute
{

    private readonly string _property;

    public EqualToPropertyAttribute(string property)
    {
        _property = property;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string)
            return ValidationResult.Success!;
        var equalTo = validationContext.ObjectInstance.GetType().GetProperty(_property);

        var propertyValue = equalTo!.GetValue(validationContext.ObjectInstance) as string;

        return value is string stringValue && stringValue.Equals(propertyValue)
            ? ValidationResult.Success! : new ValidationResult(GetErrorMessage());

    }

    private string GetErrorMessage()
    {
        if (ErrorMessageResourceType != null && !string.IsNullOrEmpty(ErrorMessageResourceName))
        {
            var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
            var errorMessage = resourceManager.GetString(ErrorMessageResourceName);
            if (errorMessage != null) return errorMessage;
        }
        return $"It is not equal to {_property}";
    }
}