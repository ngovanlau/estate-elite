namespace Core.Domain.Primitives;

public abstract class Entity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
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

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }
}