namespace Common.Infrastructure.Settings;

public class ConfirmationCodeSetting
{
    public required string AllowedChars { get; set; }
    public int CodeLength { get; set; }
    public int ExpirationTimeInMinutes { get; set; }
    public int MaximumAttempts { get; set; }
}
