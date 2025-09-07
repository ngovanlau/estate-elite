using Core.Domain.Abstractions;

namespace Core.Domain.Primitives;

public abstract class SoftDeletableEntity : AuditableEntity, ISoftDeletableEntity
{
    /// <inheritdoc />
    public DateTime? DeletedOn { get; set; }

    /// <inheritdoc />
    public Guid? DeletedBy { get; set; }

    /// <summary>
    /// Indicates whether the entity is soft-deleted.
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Protected constructor for EF Core
    /// </summary>
    protected SoftDeletableEntity() : base() { }

    /// <summary>
    /// Protected constructor for creating a new entity with a specific ID
    /// </summary>
    /// <param name="id">Entity identifier</param>
    protected SoftDeletableEntity(Guid id) : base(id) { }
}