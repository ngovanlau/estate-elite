namespace Caching.Configuration;

public class CacheSetting
{
    public const string SectionName = "Redis";

    public string ConnectionString { get; set; } = string.Empty;
    public int DatabaseId { get; set; } = 0;
    public TimeSpan DefaultExpiry { get; set; } = TimeSpan.FromHours(1);
}
