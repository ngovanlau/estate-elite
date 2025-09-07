using Common.Application.Interfaces;
using Common.Application.Settings;
using Common.Infrastructure.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Infrastructure.Extensions;

public static class MinioExtension
{
    public static IServiceCollection AddMinioService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioSetting>(configuration.GetSection("Minio"));
        services.AddSingleton<IFileStorageService, MinioStorageService>();

        return services;
    }
}
