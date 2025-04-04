using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Extensions;

using Application.Interfaces;
using Data;
using EventBus.RabbitMQ.Extensions;
using Repositories;
using Utilities;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                x => x.MigrationsAssembly("IdentityService.Infrastructure")
            )
        );

        // Event bus
        services.AddEventBusServices(configuration);

        // Repository
        services.AddScoped<IUserRepository, UserRepository>();

        // Auto mapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Dependency injection
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
