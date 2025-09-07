using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PropertyService.Infrastructure.Data;

public class PropertyContextFactory() : IDesignTimeDbContextFactory<PropertyContext>
{
    public PropertyContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PropertyService.API"))
            .AddJsonFile($"appsettings.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PropertyContext>();

        var connectionString = configuration.GetConnectionString("PostgresConnection");
        optionsBuilder.UseNpgsql(connectionString,
            p => p.MigrationsAssembly("PropertyService.Infrastructure"));

        return new PropertyContext(optionsBuilder.Options);
    }
}

