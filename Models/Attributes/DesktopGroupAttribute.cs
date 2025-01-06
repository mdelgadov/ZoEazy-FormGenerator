namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DesktopGroupAttribute(int group) : GroupAttribute(group)
{
    public const bool IsPhone = false;
    public const bool IsDesktop = !IsPhone;
}