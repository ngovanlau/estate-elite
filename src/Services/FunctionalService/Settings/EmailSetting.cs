using MailKit.Security;

namespace FunctionalService.Settings;


public class SmtpSetting
{
    public required string Server { get; set; }
    public int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string SenderEmail { get; set; }
    public required string SenderName { get; set; }
    public SecureSocketOptions SecurityOptions { get; set; } = SecureSocketOptions.Auto;
}
