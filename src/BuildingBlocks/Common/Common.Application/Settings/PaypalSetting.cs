namespace Common.Application.Settings;

public sealed record PaypalSetting(
    string ClientId,
    string ClientSecret,
    bool UseSandbox = true
)
{
    public const string SectionName = "PaypalSetting";
};
