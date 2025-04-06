using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace DistributedCache.Redis.Extensions;

public static class DistributedCacheServicesExtension
{
    public static IServiceCollection AddDistributedServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("RedisConnection");
        if (string.IsNullOrWhiteSpace(redisConnectionString))
        {
            throw new ArgumentException("Redis connection string is not configured");
        }

        services.AddStackExchangeRedisCache(options =>
        {
            var configOptions = ConfigurationOptions.Parse(redisConnectionString);
            configOptions.AbortOnConnectFail = false;

            options.ConfigurationOptions = configOptions;
        });

        return services;
    }
}