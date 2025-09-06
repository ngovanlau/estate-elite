using System.Text;
using System.Text.Json;

namespace Caching.Configuration;

public static class CacheKeys
{
    private const string Separator = ":";

    public static string ForEntity<T>(Guid entityId) where T : class
        => $"{typeof(T).Name.ToLowerInvariant()}{Separator}entity{Separator}{Separator}{entityId}";

    public static string ForCollection<TEntity, T>(string? suffix = null) where T : class
        => $"{typeof(TEntity).Name.ToLowerInvariant()}{Separator}collection{Separator}{typeof(T).Name.ToLowerInvariant()}{(string.IsNullOrWhiteSpace(suffix) ? string.Empty : $"{Separator}{suffix}")}";

    public static string ForQuery<TEntity, T>(object? queryParams = null) where T : class
    {
        string queryString = queryParams == null
            ? "all"
            : Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(queryParams)));

        return $"{typeof(TEntity).Name.ToLowerInvariant()}{Separator}query{Separator}{typeof(T).Name.ToLowerInvariant()}{Separator}{queryString}";
    }

    public static string ForDto<TEntity, T>(Guid dtoId) where T : class
    {
        var name = GetNameFromDto<T>();
        return $"{typeof(TEntity).Name.ToLowerInvariant()}{Separator}dto{Separator}{name}{Separator}{dtoId}";
    }

    public static string ForDtoCollection<TEntity, T>(string? suffix = null) where T : class
    {
        var name = GetNameFromDto<T>();
        return $"{typeof(TEntity).Name.ToLowerInvariant()}{Separator}dto_collection{Separator}{name}{(string.IsNullOrWhiteSpace(suffix) ? string.Empty : $"{Separator}{suffix}")}";
    }

    private static string GetNameFromDto<T>() where T : class
    {
        return typeof(T).Name
                    .Replace("dto", "", StringComparison.OrdinalIgnoreCase)
                    .ToLowerInvariant();
    }

    public static string ForPattern<T>() where T : class
        => $"*{typeof(T).Name.ToLowerInvariant()}*";

    public static string ForRefreshToken(Guid userId)
        => $"user{Separator}{userId}{Separator}refresh_token";
}
