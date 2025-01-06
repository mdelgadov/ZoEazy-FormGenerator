using System.ComponentModel.DataAnnotations;

namespace ZoEazy.Models;

public class Entity : IEquatable<Entity>
{
    protected Entity(long id = 0)
    {
        Id = id;
    }

    [Key] public long Id { get; set; }

    public bool Equals(Entity? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        return obj.Id == Id;
    }

    public static bool operator ==(Entity? first, Entity? second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static bool operator !=(Entity? first, Entity? second)
    {
        return !(first == second);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType() || obj is not Entity entity) return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        // hashcode * prime number
        return Id.GetHashCode() * 41;
    }
}