namespace ZoEazy.Models;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class InsertPointAttribute(int point) : MetadataValidationAttribute
{
    public int Point { get; init; } = point;
}