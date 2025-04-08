using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DistributedCache.Redis;

public static class RedisCacheService
{
    private readonly static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = null,
        WriteIndented = true,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30))
            .SetAbsoluteExpiration(TimeSpan.FromHours(1));

        return SetAsync(cache, key, value, options, cancellationToken);
    }

    public static Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, serializerOptions));
        return cache.SetAsync(key, bytes, options, cancellationToken);
    }

    public static async Task<(bool Success, T? Value)> TryGetValueAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default)
    {
        var val = await cache.GetAsync(key, cancellationToken);
        if (val == null) return (false, default);

        try
        {
            var value = JsonSerializer.Deserialize<T>(val, serializerOptions);
            return (true, value);
        }
        catch
        {
            return (false, default);
        }
    }

    public static async Task<T?> GetOrSetAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> task, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (options == null)
        {
            options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30))
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));
        }

        var (Success, Value) = await cache.TryGetValueAsync<T>(key, cancellationToken);
        if (Success && Value is not null)
        {
            return Value;
        }

        var value = await task();
        if (value is not null)
        {
            await cache.SetAsync<T>(key, value, options, cancellationToken);
        }
        return value;
    }
}
