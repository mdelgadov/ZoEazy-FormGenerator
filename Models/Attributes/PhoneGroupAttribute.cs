namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class PhoneGroupAttribute(int group) : GroupAttribute(group)
{
    public const bool IsPhone = true;
    public const bool IsDesktop = !IsPhone;
}