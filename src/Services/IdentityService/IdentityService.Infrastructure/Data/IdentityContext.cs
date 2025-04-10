using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data;

using Domain.Entities;
using SharedKernel.Entities;

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
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(30);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PasswordHash).IsRequired();

            // Additional configurations
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}
