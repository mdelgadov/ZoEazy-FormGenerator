namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PickerAttribute(string method) : MetadataValidationAttribute
{
    public string Method { get; } = method;

}