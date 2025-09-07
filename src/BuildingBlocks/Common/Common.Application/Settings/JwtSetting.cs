namespace Common.Application.Settings;

public sealed record JwtSetting(
    string SecretKey,
    string Issuer,
    string Audience,
    int AccessTokenExpirationInMinutes,
    int RefreshTokenExpirationInDays,
    int RefreshTokenSlidingExpirationInMinutes,
    string TokenType
)
{
    public const string SectionName = "JwtSetting";
}
