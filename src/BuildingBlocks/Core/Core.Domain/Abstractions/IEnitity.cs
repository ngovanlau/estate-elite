namespace Core.Domain.Abstractions;

/// <summary>
/// Base interface for all entities
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    Guid Id { get; }
}
