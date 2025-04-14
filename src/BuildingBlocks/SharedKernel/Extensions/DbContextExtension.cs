using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Entities;

namespace SharedKernel.Extensions;

public static class DbContextExtension
{
    public static IQueryable<T> Available<T>(this DbContext context, bool isTracking = true)
        where T : AuditableEntity
    {
        var result = context.Set<T>().Where(x => !x.IsDelete);
        return isTracking ? result : result.AsNoTracking();
    }

    public static void ConfigureAuditProperties<T>(this EntityTypeBuilder<T> entity) where T : AuditableEntity
    {
        // Primary key
        entity.HasKey(e => e.Id);

        entity.Property(p => p.CreatedBy)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(p => p.CreatedOn)
            .IsRequired();

        entity.Property(p => p.ModifiedBy)
            .HasMaxLength(50);

        entity.Property(p => p.ModifiedOn);

        entity.Property(p => p.IsDelete)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
