using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DistributedCache.Redis.Extensions;

public static class DistributedCacheServicesExtension
{
    public static IServiceCollection AddDistributedService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("RedisConnection");
        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new ArgumentException("Redis connection string is not configured");
        }

        // Tạo ConfigurationOptions từ connection string
        var configOptions = ConfigurationOptions.Parse(redisConnectionString);
        configOptions.AbortOnConnectFail = false;

        // Đăng ký IConnectionMultiplexer như một Singleton
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<object>>();
            try
            {
                var connection = ConnectionMultiplexer.Connect(configOptions);
                logger.LogInformation("Connected to Redis successfully");
                return connection;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to connect to Redis");
                throw;
            }
        });

        // Đăng ký Redis distributed cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = configOptions;
        });

        return services;
    }
}