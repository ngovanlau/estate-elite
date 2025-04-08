using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Infrastructure.Data;

public class IdentityContextFactory() : IDesignTimeDbContextFactory<IdentityContext>
{
    public IdentityContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../IdentityService.API"))
            .AddJsonFile($"appsettings.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();

        var connectionString = configuration.GetConnectionString("PostgresConnection");
        optionsBuilder.UseNpgsql(connectionString,
            p => p.MigrationsAssembly("IdentityService.Infrastructure"));

        return new IdentityContext(optionsBuilder.Options);
    }
}

