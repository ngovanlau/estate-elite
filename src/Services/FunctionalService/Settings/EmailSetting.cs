using System;
using MailKit.Security;

namespace FunctionalService.Settings;

public class SmtpSetting
{
    public string? Server { get; set; }
    public int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? SenderEmail { get; set; }
    public string? SenderName { get; set; }
    public SecureSocketOptions SecurityOptions { get; set; } = SecureSocketOptions.Auto;
}
