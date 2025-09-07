using Core.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Infrastructure.Extensions;

/// <summary>
/// Extension methods for DbContext
/// </summary>
public static class DbContextExtension
{
    /// <summary>
    /// Get available (not soft-deleted) entities with optional tracking
    /// </summary>
    /// <typeparam name="T">The type of the entity</typeparam>
    /// <param name="context">The database context</param>
    /// <param name="isTracking">Whether to track changes to the entities</param>
    /// <returns>A queryable collection of available entities</returns>
    public static IQueryable<T> Available<T>(this DbContext context, bool isTracking = true)
        where T : class, ISoftDeletableEntity
    {
        var result = context.Set<T>().Where(x => !x.IsDeleted);
        return isTracking ? result : result.AsNoTracking();
    }

    /// <summary>
    /// Configure common properties for all entities
    /// </summary>
    /// <typeparam name="T">The type of the entity</typeparam>
    /// <param name="entity">The entity type builder</param>
    public static void ConfigureEntityProperties<T>(this EntityTypeBuilder<T> entity)
        where T : class, IEntity
    {
        // Primary key
        entity.HasKey(e => e.Id);
    }

    /// <summary>
    /// Configure audit properties for all auditable entities
    /// </summary>
    /// <typeparam name="T">The type of the entity</typeparam>
    /// <param name="entity">The entity type builder</param>
    public static void ConfigureAuditProperties<T>(this EntityTypeBuilder<T> entity)
        where T : class, IEntity, IAuditableEntity
    {
        entity.ConfigureEntityProperties();

        entity.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(p => p.CreatedOn)
            .IsRequired();

        entity.Property(p => p.ModifiedBy)
            .HasMaxLength(50);

        entity.Property(p => p.ModifiedOn);
    }

    /// <summary>
    /// Configure soft delete properties for all soft deletable entities
    /// </summary>
    /// <typeparam name="T">The type of the entity</typeparam>
    /// <param name="entity">The entity type builder</param>
    public static void ConfigureSoftDeleteProperties<T>(this EntityTypeBuilder<T> entity)
        where T : class, IEntity, IAuditableEntity, ISoftDeletableEntity
    {
        entity.ConfigureAuditProperties();

        entity.Property(p => p.DeletedBy)
            .HasMaxLength(50);

        entity.Property(p => p.DeletedOn);

        entity.Property(p => p.IsDeleted)
            .IsRequired();

        // Global Query Filter that automatically filters out soft-deleted entities
        entity.HasQueryFilter(e => !e.IsDeleted);
    }
}
