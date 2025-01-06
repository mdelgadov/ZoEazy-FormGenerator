namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class HiddenAttribute : MetadataValidationAttribute
{
    public bool IsHidden = true;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PasswordAttribute : MetadataValidationAttribute
{
    public bool IsPassword = true;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class RadioAttribute : MetadataValidationAttribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class BarsAttribute : MetadataValidationAttribute
{
}