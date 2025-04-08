namespace SharedKernel.Constants;

public class JwtSetting
{
    public required string SecretKey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required int ExpirationInMinutes { get; set; }
    public required int RefreshTokenExpirationInDays { get; set; }
    public required int RefreshTokenSlidingExpirationInMinutes { get; set; }
    public required string TokenType { get; set; }
}
