using System.Text;
using System.Text.Json;

namespace DistributedCache.Redis;

public static class CacheKeys
{
    private const string Separator = ":";

    public static string ForEntity<T>(object entityId) where T : class
        => $"entity{Separator}{typeof(T).Name.ToLowerInvariant()}{Separator}{entityId}";

    public static string ForCollection<T>(string? suffix = null) where T : class
        => $"collection{Separator}{typeof(T).Name.ToLowerInvariant()}{(string.IsNullOrWhiteSpace(suffix) ? string.Empty : $"{Separator}{suffix}")}";

    public static string ForQuery<T>(object? queryParams = null) where T : class
    {
        string queryString = queryParams == null
            ? "all"
            : Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(queryParams)));

        return $"query{Separator}{typeof(T).Name.ToLowerInvariant()}{Separator}{queryString}";
    }

    public static string ForDto<T>(object dtoId) where T : class
        => $"dto{Separator}{GetNameFromDto<T>}{Separator}{dtoId}";

    public static string ForDtoCollection<T>(string? suffix = null) where T : class
        => $"dto_collection{Separator}{GetNameFromDto<T>()}{(string.IsNullOrWhiteSpace(suffix) ? string.Empty : $"{Separator}{suffix}")}";

    public static string ForDtoQuery<T>(object? queryParams = null) where T : class
    {
        string queryString = queryParams == null
            ? "all"
            : Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(queryParams)));

        return $"dto_query{Separator}{GetNameFromDto<T>()}{Separator}{queryString}";
    }

    private static string GetNameFromDto<T>() where T : class
    {
        return typeof(T).Name
                    .Replace("dto", "", StringComparison.OrdinalIgnoreCase)
                    .ToLowerInvariant();
    }

    public static string ForPattern<T>() where T : class
        => $"*{typeof(T).Name.ToLowerInvariant()}*";

    public static string ForUser(string userId, string? action = null)
        => $"user{Separator}{userId}{(string.IsNullOrWhiteSpace(action) ? string.Empty : $"{Separator}{action}")}";
}
