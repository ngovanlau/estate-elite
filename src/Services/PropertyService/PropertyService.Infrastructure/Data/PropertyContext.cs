using Microsoft.EntityFrameworkCore;

namespace PropertyService.Infrastructure.Data;

using Domain.Entities;
using Common.Domain.Enums;
using Common.Infrastructure.Extensions;

public class PropertyContext(DbContextOptions<PropertyContext> options) : DbContext(options)
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<PropertyRoom> PropertyRooms { get; set; }
    public DbSet<Utility> Utilities { get; set; }
    public DbSet<PropertyUtility> PropertyUtilities { get; set; }
    public DbSet<PropertyRental> PropertyRentals { get; set; }
    public DbSet<PropertyView> PropertyViews { get; set; }

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
            entity.Property(e => e.Details)
                .HasMaxLength(200);

            // Configure double properties with precision and scale
            entity.Property(e => e.Latitude)
                .HasPrecision(18, 9);

            entity.Property(e => e.Longitude)
                .HasPrecision(18, 9);

            entity.Property(e => e.GooglePlaceId)
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

            entity.Property(e => e.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            // Optional properties
            entity.Property(e => e.Caption)
                .HasMaxLength(500);

            entity.Property(e => e.PropertyId);

            entity.Property(e => e.ProjectId);

            // Indexes
            entity.HasIndex(e => e.HashId);
            entity.HasIndex(e => e.ProjectId);
            entity.HasIndex(e => new { e.ProjectId, e.IsMain });
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

            entity.HasMany(e => e.Images)
                .WithOne()
                .HasForeignKey(e => e.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

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
                .HasDefaultValue(CurrencyUnit.USD)
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
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Images)
                .WithOne()
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Views)
                .WithOne()
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship with PropertyUtility (many-to-many)
            entity.HasMany(e => e.Utilities)
                .WithMany(p => p.Properties)
                .UsingEntity<PropertyUtility>(
                    j => j.HasOne(pu => pu.Utility)
                        .WithMany()
                        .HasForeignKey(pu => pu.UtilityId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne(pu => pu.Property)
                        .WithMany()
                        .HasForeignKey(pu => pu.PropertyId)
                        .OnDelete(DeleteBehavior.Cascade)
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

        modelBuilder.Entity<Room>(entity =>
        {
            // Table name
            entity.ToTable("Rooms");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Required properties
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Index
            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<PropertyRoom>(entity =>
        {
            // Table name
            entity.ToTable("PropertyRooms");

            // Primary key
            entity.HasKey(e => new { e.PropertyId, e.RoomId });

            // Required properties
            entity.Property(e => e.Quantity);

            // Relationships
            entity.HasOne(e => e.Property)
                .WithMany(p => p.PropertyRooms)
                .HasForeignKey(e => e.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Room)
                .WithMany(p => p.PropertyRooms)
                .HasForeignKey(e => e.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
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

        modelBuilder.Entity<PropertyRental>(entity =>
        {
            // Table name
            entity.ToTable("PropertyRentals");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Configure required properties
            entity.Property(e => e.UserId)
                .IsRequired();

            entity.Property(e => e.StartDate)
                .IsRequired();

            entity.Property(e => e.EndDate)
                .IsRequired();

            entity.Property(e => e.PropertyId)
                .IsRequired();

            // Configure required relationships
            entity.HasOne(e => e.Property)
                .WithMany()
                .HasForeignKey(e => e.PropertyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Index
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.PropertyId);
            entity.HasIndex(e => new { e.StartDate, e.EndDate });
        });

        modelBuilder.Entity<PropertyView>(entity =>
        {
            // Table name
            entity.ToTable("PropertyViews");

            // Audit properties (inherited from AuditableEntity)
            entity.ConfigureAuditProperties();

            // Primary key
            entity.HasKey(pv => pv.Id);

            // Required properties
            entity.Property(pv => pv.IpAddress)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(pv => pv.PropertyId)
                .IsRequired();

            // Optional properties
            entity.Property(pv => pv.UserId);

            entity.Property(pv => pv.UserAgent)
                .HasMaxLength(500);
        });
    }
}
