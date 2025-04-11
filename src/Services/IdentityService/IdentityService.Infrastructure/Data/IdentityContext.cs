using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

using Domain.Entities;
using SharedKernel.Extensions;

public class IdentityContext(DbContextOptions<IdentityContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the default schema for the entire model
        modelBuilder.HasDefaultSchema("identity");

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            // Table name
            entity.ToTable("Users");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Properties
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(30);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.Property(e => e.Role)
                .IsRequired()
                .HasConversion<string>();

            // Optional properties
            entity.Property(u => u.Phone)
                .HasMaxLength(20);

            entity.Property(u => u.Address)
                .HasMaxLength(255);

            entity.Property(u => u.Avatar)
                .HasMaxLength(255);

            entity.Property(u => u.Background)
                .HasMaxLength(255);

            // Indexes
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Role);
        });
    }
}
