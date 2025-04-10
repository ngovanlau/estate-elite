using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace SharedKernel.Extensions;

public static class OpenApiExtension
{
    public static IServiceCollection AddOpenApiService(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Identity Service API",
                Version = "v1"
            });
        });

        return services;
    }
}
