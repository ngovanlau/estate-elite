using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Data;
using PaymentService.Infrastructure.PaymentProviders.Paypal;
using PaymentService.Infrastructure.Repositories;
using PaymentService.Infrastructure.Services;

namespace PaymentService.Infrastructure.Extensions;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PaymentContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                x => x.MigrationsAssembly("PaymentService.Infrastructure")
            )
        );

        // Repository
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        // Auto mapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Dependency injection
        services.AddScoped<IPaypalService, PaypalService>();
        services.AddSingleton<PaypalClientFactory>();

        return services;
    }
}
