using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using Common.Infrastructure.Extensions;

namespace PaymentService.Infrastructure.Data;

public class PaymentContext(DbContextOptions<PaymentContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the default schema for the entire model
        modelBuilder.HasDefaultSchema("payment");

        // Configure Transaction entity
        modelBuilder.Entity<Transaction>(entity =>
        {
            // Table name
            entity.ToTable("Transactions");

            // Audit properties (assuming these are in AuditableEntity)
            entity.ConfigureAuditProperties();

            // Required properties
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            entity.Property(e => e.CurrencyUnit)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.PaymentMethod)
                .IsRequired()
                .HasConversion<string>();

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.Property(e => e.PropertyId)
                .IsRequired();

            entity.Property(e => e.RentalPeriod)
                .IsRequired();
        });
    }
}
