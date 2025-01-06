namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class GroupAttribute(int group) : MetadataValidationAttribute
{
    public int Group { get; init; } = group;
}