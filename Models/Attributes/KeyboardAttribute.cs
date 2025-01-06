namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class KeyboardAttribute(KeyboardType keyboardType = KeyboardType.Default) : MetadataValidationAttribute
{
    public KeyboardType KeyboardType { get; } = keyboardType;
}