using System.Text;
using System.Text.Json;

namespace DistributedCache.Redis;

public static class CacheKeys
{
    private const string Separator = ":";

    public static string ForEntity<T>(Guid entityId) where T : class
        => $"entity{Separator}{typeof(T).Name.ToLowerInvariant()}{Separator}{entityId}";

    public static string ForCollection<T>(string? suffix = null) where T : class
        => $"collection{Separator}{typeof(T).Name.ToLowerInvariant()}{(string.IsNullOrWhiteSpace(suffix) ? string.Empty : $"{Separator}{suffix}")}";

    public static string ForQuery<T>(Guid? queryParams = null) where T : class
    {
        string queryString = queryParams == null
            ? "all"
            : Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(queryParams)));

        return $"query{Separator}{typeof(T).Name.ToLowerInvariant()}{Separator}{queryString}";
    }

    public static string ForDto<T>(Guid dtoId) where T : class
    {
        var name = GetNameFromDto<T>();
        return $"dto{Separator}{name}{Separator}{dtoId}";
    }

    public static string ForDtoCollection<T>(string? suffix = null) where T : class
    {
        var name = GetNameFromDto<T>();
        return $"dto_collection{Separator}{name}{(string.IsNullOrWhiteSpace(suffix) ? string.Empty : $"{Separator}{suffix}")}";
    }

    public static string ForDtoQuery<T>(object? queryParams = null) where T : class
    {
        string queryString = queryParams == null
            ? "all"
            : Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(queryParams)));
        var name = GetNameFromDto<T>();
        return $"dto_query{Separator}{name}{Separator}{queryString}";
    }

    private static string GetNameFromDto<T>() where T : class
    {
        return typeof(T).Name
                    .Replace("dto", "", StringComparison.OrdinalIgnoreCase)
                    .ToLowerInvariant();
    }

    public static string ForPattern<T>() where T : class
        => $"*{typeof(T).Name.ToLowerInvariant()}*";
}
