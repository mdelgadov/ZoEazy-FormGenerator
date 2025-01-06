using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MustBeTrueAttribute : MetadataValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        return value is null or false ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success!;
    }
    private string GetErrorMessage()
    {
        if (ErrorMessageResourceType == null || string.IsNullOrEmpty(ErrorMessageResourceName))
            return "Must check to accept";

        var resourceManager = new System.Resources.ResourceManager(ErrorMessageResourceType);
        var errorMessage = resourceManager.GetString(ErrorMessageResourceName) ?? string.Empty;
        return string.Format(errorMessage);
    }
}