using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Caching.Extensions;

public static class CacheServicesExtension
{
    public static IServiceCollection AddCacheService(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("RedisConnection");
        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new ArgumentException("Redis connection string is not configured");
        }

        // Create ConfigurationOptions from connection string
        var configOptions = ConfigurationOptions.Parse(redisConnectionString);
        configOptions.AbortOnConnectFail = false;

        // Register ConnectionMultiplexer as a singleton
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
