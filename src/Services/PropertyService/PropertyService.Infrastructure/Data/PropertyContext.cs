using Microsoft.EntityFrameworkCore;
using SharedKernel.Entities;

namespace PropertyService.Infrastructure.Data;

public class PropertyContext(DbContextOptions<PropertyContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set the default schema for the entire model
        modelBuilder.HasDefaultSchema("property");

        // Configure entity
    }
}
