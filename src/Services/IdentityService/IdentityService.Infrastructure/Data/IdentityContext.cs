using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

using Domain.Entities;
using SharedKernel.Extensions;

public class IdentityContext(DbContextOptions<IdentityContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<SellerProfile> SellerProfiles { get; set; }

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

        modelBuilder.Entity<SellerProfile>(entity =>
        {
            // Table name
            entity.ToTable("SellerProfiles");

            entity.HasKey(e => e.UserId);

            // Required properties
            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.TaxIdentificationNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(e => e.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(p => p.CreatedBy)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.CreatedOn)
                .IsRequired();

            entity.Property(p => p.IsDelete)
                .IsRequired()
                .HasDefaultValue(false);

            // Optional properties
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(50);

            entity.Property(e => e.ProfessionalLicense)
                .HasMaxLength(50);

            entity.Property(e => e.Biography)
                .HasMaxLength(1000);

            entity.Property(p => p.ModifiedBy)
                .HasMaxLength(50);

            entity.Property(p => p.ModifiedOn);

            // Primary key and relationship
            entity.HasKey(e => e.UserId);

            entity.HasOne(e => e.User)
                .WithOne(u => u.SellerProfile)
                .HasForeignKey<SellerProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            entity.HasIndex(e => e.TaxIdentificationNumber).IsUnique();
            entity.HasIndex(e => e.CompanyName);
        });
    }
}
