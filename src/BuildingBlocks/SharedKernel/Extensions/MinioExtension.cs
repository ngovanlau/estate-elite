using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Implements;
using SharedKernel.Interfaces;
using SharedKernel.Settings;

namespace SharedKernel.Extensions;

public static class MinioExtension
{
    public static IServiceCollection AddMinioService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioSetting>(configuration.GetSection("Minio"));
        services.AddSingleton<IFileStorageService, MinioStorageService>();

        return services;
    }
}
