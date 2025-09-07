/*
 * Why define Entity this way:
 * ---------------------------
 * 1. Identity:
 *    - Every entity has a unique Id (Guid) that distinguishes it from others.
 *    - Id is immutable after creation (`private init`).
 *
 * 2. Constructors:
 *    - Protected default constructor: required by EF Core for materialization.
 *    - Protected constructor with explicit Guid: allows controlled initialization.
 *
 * 3. Static Factory Methods:
 *    - Create<T>() → generates a new entity with a new Guid Id.
 *    - Create<T>(Guid id) → generates entity with a specific Guid Id.
 *    - Ensures consistent creation logic across all entities.
 *
 * 4. ToString():
 *    - Overrides default output for easier debugging/logging (prints type + Id).
 *
 * 5. Equality Handling:
 *    - Entities are compared by identity (Id), not by reference or property values.
 *    - Operators == and != are overridden for intuitive equality checks.
 *    - Equals/GetHashCode ensure correct behavior in collections (e.g., HashSet, Dictionary).
 *    - Equality also requires the same runtime type (avoids comparing different entity types with the same Id).
 *
 * 6. DDD Principle:
 *    - Entities are defined by their identity, not their attributes.
 *    - Two entities with the same Id are considered equal, even if other properties differ.
 *
 * Overall:
 * Entity provides a consistent, reusable base class for all domain entities,
 * enforcing identity-based equality and simplifying creation and debugging.
 */

using Core.Domain.Abstractions;

namespace Core.Domain.Primitives;

public abstract class Entity : IEntity, IEquatable<Entity>
{
    /// <inheritdoc />
    public Guid Id { get; private init; }

    /// <summary>
    /// Protected constructor for EF Core
    /// </summary>
    protected Entity()
    {
    }

    /// <summary>
    /// Protected constructor with explicit ID
    /// </summary>
    /// <param name="id">Entity identifier</param>
    protected Entity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Static factory method to create entity with new ID
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <returns>New entity instance</returns>
    protected static T Create<T>() where T : Entity, new()
    {
        return new T { Id = Guid.NewGuid() };
    }

    /// <summary>
    /// Static factory method to create entity with specific ID
    /// </summary>
    /// <typeparam name="T">Type of the entity</typeparam>
    /// <param name="id">Entity identifier</param>
    /// <returns>New entity instance</returns>
    protected static T Create<T>(Guid id) where T : Entity, new()
    {
        return new T { Id = id };
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    #region Equality

    public static bool operator ==(Entity? left, Entity? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(Entity? left, Entity? right)
        => !(left == right);

    public bool Equals(Entity? other)
    {
        if (other is null || GetType() != other.GetType())
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id == other.Id;
    }

    public override bool Equals(object? obj)
        => obj is Entity other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);

    #endregion
}
