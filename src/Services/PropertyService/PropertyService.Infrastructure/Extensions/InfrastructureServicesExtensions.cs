using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Infrastructure.Extensions;

using Application.Interfaces;
using Data;
using PropertyService.Infrastructure.Utilities;
using Repositories;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PropertyContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("PostgresConnection"),
                x => x.MigrationsAssembly("PropertyService.Infrastructure")
            )
        );

        // Repository
        services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
        services.AddScoped<IUtilityRepository, UtilityRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IPropertyRentalRepository, PropertyRentalRepository>();

        // Auto mapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Dependency injection
        services.AddScoped<IPropertyViewRepository, PropertyViewRepository>();
        services.AddScoped<IViewTracker, ViewTracker>();

        return services;
    }
}
