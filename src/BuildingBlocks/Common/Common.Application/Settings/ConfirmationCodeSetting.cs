namespace Common.Application.Settings;

public sealed record ConfirmationCodeSetting(
    string AllowedChars,
    int CodeLength,
    int ExpirationTimeInMinutes,
    int MaximumAttempts
)
{
    public const string SectionName = "ConfirmationCodeSetting";
}
