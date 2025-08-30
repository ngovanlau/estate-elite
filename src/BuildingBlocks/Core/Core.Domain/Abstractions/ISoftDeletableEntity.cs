/*
 * Why define ISoftDeletableEntity (soft delete pattern):
 * -----------------------------------------------------
 * 1. Soft Delete vs Hard Delete:
 *    - Hard delete: record is physically removed from the database.
 *    - Soft delete: record remains but marked as deleted, allowing recovery,
 *      auditing, and preserving referential integrity.
 *
 * 2. Standardization:
 *    - Provides a common contract for entities that support soft deletion.
 *    - Ensures consistent fields: DeletedOn, DeletedBy, IsDeleted.
 *
 * 3. Separation of Concerns:
 *    - Business/domain logic only marks an entity as deleted (IsDeleted = true).
 *    - Infrastructure (e.g., EF Core global filters) can automatically exclude
 *      deleted records from queries.
 *
 * 4. Auditing & Compliance:
 *    - Retains who deleted the entity and when.
 *    - Useful for audit logs, data recovery, and regulatory requirements.
 *
 * 5. Nullable DeletedOn/DeletedBy:
 *    - Entity may never be deleted, so these fields are optional.
 *
 * Overall:
 * ISoftDeletableEntity provides a clean, consistent contract for implementing
 * soft deletion across entities, improving maintainability, auditability, and safety.
 */

namespace Core.Domain.Abstractions;

/// <summary>
/// Interface for soft-deletable entities.
/// </summary>
public interface ISoftDeletableEntity
{
    /// <summary>
    /// When the entity was deleted.
    /// </summary>
    DateTime? DeletedOn { get; set; }

    /// <summary>
    /// Who deleted the entity.
    /// </summary>
    string? DeletedBy { get; set; }

    /// <summary>
    /// Indicates whether the entity is deleted.
    /// </summary>
    bool IsDeleted { get; set; }
}
