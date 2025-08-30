/*
 * Why this interface exists (IAuditableEntity):
 * --------------------------------------------
 * 1. Standardization:
 *    - Defines a common contract for all entities that require auditing.
 *    - Ensures every auditable entity has CreatedOn, ModifiedOn, CreatedBy, ModifiedBy.
 *
 * 2. Separation of Concerns:
 *    - Business/domain logic does not need to know how auditing is implemented.
 *    - Infrastructure (e.g., EF Core interceptors, middleware, repositories) can
 *      automatically populate or update these fields without touching business code.
 *
 * 3. Flexibility:
 *    - Any entity that needs auditing simply implements IAuditableEntity.
 *    - Consistent handling across the system (no duplication of audit properties).
 *
 * 4. Use Cases:
 *    - Track who created/modified entities and when.
 *    - Support auditing, logging, change tracking, and compliance.
 *
 * 5. Nullable ModifiedOn/ModifiedBy:
 *    - Newly created entities may not have modification info yet.
 *
 * Overall:
 * IAuditableEntity acts as a shared contract that makes it easy to implement
 * cross-cutting concerns like auditing in a clean and consistent way.
 */

namespace Core.Domain.Abstractions;

/// <summary>
/// Interface for auditable entities.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// When the entity was created.
    /// </summary>
    DateTime CreateOn { get; set; }

    /// <summary>
    /// When the entity was last modified.
    /// </summary>
    DateTime? ModifiedOn { get; set; }

    /// <summary>
    /// Who created the entity.
    /// </summary>
    string? CreatedBy { get; set; }

    /// <summary>
    /// Who last modified the entity.
    /// </summary>
    string? ModifiedBy { get; set; }
}
