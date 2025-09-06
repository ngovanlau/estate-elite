using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Common.Infrastructure.Implements;
using Common.Application.Interfaces;
using Common.Infrastructure.Settings;

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
