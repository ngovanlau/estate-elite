/*
 * Why define AuditableEntity this way:
 * ------------------------------------
 * 1. Inherits from Entity:
 *    - Still an entity with identity (Id).
 *    - Adds audit properties required for tracking lifecycle events.
 *
 * 2. Implements IAuditableEntity:
 *    - Ensures all auditable entities share the same contract (CreateOn, ModifiedOn, CreatedBy, ModifiedBy).
 *    - Promotes consistency across the domain.
 *
 * 3. Audit Fields:
 *    - CreateOn / CreatedBy  → track who created the entity and when.
 *    - ModifiedOn / ModifiedBy → track the last modification.
 *    - Nullable ModifiedOn/ModifiedBy → new entities may not have been modified yet.
 *
 * 4. Constructors:
 *    - Protected default constructor: required by EF Core when materializing entities.
 *    - Protected constructor with explicit Guid Id: allows controlled initialization.
 *    - Both constructors automatically set CreateOn = DateTime.UtcNow at creation time.
 *
 * 5. Separation of Concerns:
 *    - Business logic focuses on domain rules.
 *    - Infrastructure (e.g., EF Core SaveChanges interceptor) can automatically set CreatedBy/ModifiedBy values.
 *
 * Overall:
 * AuditableEntity provides a reusable base for entities that require auditing,
 * ensuring consistency, reducing duplication, and making it easy to integrate
 * auditing concerns into persistence and application layers.
 */

using Core.Domain.Abstractions;

namespace Core.Domain.Primitives;

public abstract class AuditableEntity : Entity, IAuditableEntity
{
    /// <inheritdoc />
    public DateTime CreatedOn { get; set; }

    /// <inheritdoc />
    public DateTime? ModifiedOn { get; set; }

    /// <inheritdoc />
    public Guid? CreatedBy { get; set; }

    /// <inheritdoc />
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    /// Protected constructor for EF Core
    /// </summary>
    protected AuditableEntity()
    {
        CreatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Protected constructor with explicit ID
    /// </summary>
    protected AuditableEntity(Guid id) : base(id)
    {
        CreatedOn = DateTime.UtcNow;
    }
}