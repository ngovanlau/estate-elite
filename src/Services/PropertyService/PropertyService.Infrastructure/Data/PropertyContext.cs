using Microsoft.EntityFrameworkCore;

namespace PropertyService.Infrastructure.Data;

using Domain.Entities;
using SharedKernel.Extensions;

public class PropertyContext(DbContextOptions<PropertyContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the default schema for the entire model
        modelBuilder.HasDefaultSchema("property");

        // Configure entity
        modelBuilder.Entity<Address>(entity =>
        {
            // Table name
            entity.ToTable("Addresses");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Required properties
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Province)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.District)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Ward)
                .IsRequired()
                .HasMaxLength(100);

            // Optional properties
            entity.Property(e => e.Street)
                .HasMaxLength(200);

            entity.Property(e => e.StreetNumber)
                .HasMaxLength(20);

            // Configure decimal properties with precision and scale
            entity.Property(e => e.Latitude)
                .HasPrecision(18, 9);

            entity.Property(e => e.Longitude)
                .HasPrecision(18, 9);

            entity.Property(e => e.GooglePlaceId)
                .IsRequired()
                .HasMaxLength(100);

            // Index for geolocation queries
            entity.HasIndex(e => new { e.Latitude, e.Longitude });

            // Index for place ID lookups
            entity.HasIndex(e => e.GooglePlaceId);

            // Composite index for address hierarchy lookups
            entity.HasIndex(e => new { e.Country, e.Province, e.District, e.Ward });
        });

        modelBuilder.Entity<Image>(entity =>
        {
            // Table name
            entity.ToTable("Images");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Alternative key
            entity.HasAlternateKey(e => e.HashId);

            // Required properties
            entity.Property(e => e.HashId)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.OriginalFilename)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.BucketName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.ObjectName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.FileSize)
                .IsRequired()
                .HasPrecision(18, 2);

            // Optional properties
            entity.Property(e => e.Caption)
                .HasMaxLength(500);

            entity.Property(e => e.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(e => e.EntityId)
                .IsRequired();

            // Indexes
            entity.HasIndex(e => e.HashId);
            entity.HasIndex(e => e.EntityId);
            entity.HasIndex(e => new { e.EntityId, e.IsMain });
        });

        modelBuilder.Entity<Project>(entity =>
        {
            // Table name
            entity.ToTable("Projects");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Required properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(e => e.DeveloperId)
                .IsRequired();

            entity.Property(e => e.TotalArea)
                .HasPrecision(18, 2);

            entity.Property(e => e.TotalUnits)
                .IsRequired();

            entity.Property(e => e.CompletionDate)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.AddressId)
                .IsRequired();

            // Relationships
            entity.HasOne(e => e.Address)
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.DeveloperId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.AddressId);
            entity.HasIndex(e => e.CompletionDate);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            // Table name
            entity.ToTable("Properties");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Required properties
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(5000);

            entity.Property(e => e.ShortDescription)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.ListingType)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.RentPeriod)
                .HasConversion<string>();

            entity.Property(e => e.Area)
                .HasPrecision(18, 2);

            entity.Property(e => e.LandArea)
                .HasPrecision(18, 2);

            entity.Property(e => e.BuildDate)
                .IsRequired();

            entity.Property(e => e.Price)
                .HasPrecision(18, 2);

            entity.Property(e => e.CurrencyUnit)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.OwnerId)
                .IsRequired();

            entity.Property(e => e.PropertyTypeId)
                .IsRequired();

            entity.Property(e => e.AddressId)
                .IsRequired();

            // Relationships
            entity.HasOne(e => e.Type)
                .WithMany()
                .HasForeignKey(e => e.PropertyTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Address)
                .WithMany()
                .HasForeignKey(e => e.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship with PropertyUtility (many-to-many)
            entity.HasMany(e => e.Utilities)
                .WithMany(p => p.Properties)
                .UsingEntity<PropertyUtility>(
                    j => j.HasOne(pu => pu.Utility)
                          .WithMany()
                          .HasForeignKey(pu => pu.UtilityId)
                          .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(pu => pu.Property)
                          .WithMany()
                          .HasForeignKey(pu => pu.PropertyId)
                          .OnDelete(DeleteBehavior.Restrict)
                );

            // Indexes
            entity.HasIndex(e => e.Title);
            entity.HasIndex(e => e.ListingType);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.OwnerId);
            entity.HasIndex(e => e.PropertyTypeId);
            entity.HasIndex(e => e.AddressId);
            entity.HasIndex(e => e.Price);
            entity.HasIndex(e => e.Area);
        });

        modelBuilder.Entity<PropertyType>(entity =>
        {
            // Table name
            entity.ToTable("PropertyTypes");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Required properties
            entity.Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<PropertyUtility>(entity =>
        {
            // Table name
            entity.ToTable("PropertyUtilities");

            // Primary key
            entity.HasKey(e => new { e.PropertyId, e.UtilityId });

            // Required properties
            entity.Property(e => e.Count);

            // Relationships
            entity.HasOne(e => e.Property)
                .WithMany()
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Utility)
                .WithMany()
                .HasForeignKey(e => e.UtilityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Utility>(entity =>
        {
            // Table name
            entity.ToTable("Utilities");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Required properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            // Relationship with PropertyUtility (many-to-many)
            entity.HasMany(e => e.Properties)
                .WithMany(p => p.Utilities)
                .UsingEntity<PropertyUtility>(
                    j => j.HasOne(pu => pu.Property)
                          .WithMany()
                          .HasForeignKey(pu => pu.PropertyId)
                          .OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne(pu => pu.Utility)
                          .WithMany()
                          .HasForeignKey(pu => pu.UtilityId)
                          .OnDelete(DeleteBehavior.Restrict)
                );

            // Index
            entity.HasIndex(e => e.Name);
        });
    }
}
