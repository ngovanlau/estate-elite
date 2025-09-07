namespace Common.Application.Settings;

public sealed record GoogleSetting(
    string ClientId,
    string ClientSecret
)
{
    public const string SectionName = "GoogleSetting";
}
