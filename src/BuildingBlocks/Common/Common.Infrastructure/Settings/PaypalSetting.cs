namespace Common.Infrastructure.Settings;

public class PaypalSetting
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public bool UseSandbox { get; set; } = true;
}
