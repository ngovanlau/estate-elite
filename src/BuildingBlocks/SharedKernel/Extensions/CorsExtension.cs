using System;
using Microsoft.Extensions.DependencyInjection;

namespace SharedKernel.Extensions;

public static class CorsExtension
{
    public static IServiceCollection AddCorsService(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });

        return services;
    }
}
