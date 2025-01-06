using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]

// Observable Validator purges out common attributes. We need the metadata accessed by the AutoFormView
// so, this thing always validates them as true.
// it's a hack, but life is full of hacks... 
public class MetadataValidationAttribute: ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        return ValidationResult.Success!;
    }
}