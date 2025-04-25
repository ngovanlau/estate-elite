using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PaymentService.Infrastructure.Data;

public class PaymentContextFactory() : IDesignTimeDbContextFactory<PaymentContext>
{
    public PaymentContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../PaymentService.API"))
            .AddJsonFile($"appsettings.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();

        var connectionString = configuration.GetConnectionString("PostgresConnection");
        optionsBuilder.UseNpgsql(connectionString,
            p => p.MigrationsAssembly("PaymentService.Infrastructure"));

        return new PaymentContext(optionsBuilder.Options);
    }
}

