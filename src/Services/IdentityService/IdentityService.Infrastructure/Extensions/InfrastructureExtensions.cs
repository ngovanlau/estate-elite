using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Infrastructure.Extensions;

using Data;
using Application.Interfaces;
using Repositories;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                x => x.MigrationsAssembly("IdentityService.Infrastructure")
            )
        );

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
