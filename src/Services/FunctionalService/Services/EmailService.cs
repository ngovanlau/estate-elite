using Microsoft.Extensions.Options;

namespace FunctionalService.Services;

using Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Settings;

public class EmailService(IOptions<SmtpSetting> options, ILogger<EmailService> logger) : IEmailService
{
    private readonly SmtpSetting _emailSetting = options.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendConfirmationCodeAsync(string email, string fullname, string confirmationCode, TimeSpan expiryTime)
    {
        var subject = "Your Email Confirmation Code";
        var body = $@"
            <html>
                <body>
                    <h2>Welcome, {fullname}!</h2>
                    <p>Thank you for registering. Please use the following code to confirm your email address:</p>
                    <h3 style='background-color: #f5f5f5; padding: 10px; text-align: center; font-family: monospace;'>{confirmationCode}</h3>
                    <p>This code will expire in {expiryTime.Minutes} minutes.</p>
                    <p>If you did not request this code, please ignore this email.</p>
                </body>
            </html>
        ";

        await SendEmailAsync(email, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        _logger.LogInformation("Preparing to send email to {Email} with subject {Subject}", toEmail, subject);

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSetting.SenderName, _emailSetting.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Enable logging of SMTP client details
                client.MessageSent += (sender, args) =>
                {
                    _logger.LogInformation("Email sent successfully to {Email} with message id {MessageId}",
                            toEmail, args.Message.MessageId);
                };

                // Connect to SMTP server with secure connection if required
                _logger.LogDebug("Connecting to SMTP server {Server}:{Port}", _emailSetting.Server, _emailSetting.Port);
                await client.ConnectAsync(_emailSetting.Server, _emailSetting.Port, _emailSetting.SecurityOptions);

                // Authenticate if credentials provided
                if (!string.IsNullOrWhiteSpace(_emailSetting.Username) && !string.IsNullOrWhiteSpace(_emailSetting.Password))
                {
                    _logger.LogDebug("Authenticating with SMTP server using username {Username}", _emailSetting.Username);
                    await client.AuthenticateAsync(_emailSetting.Username, _emailSetting.Password);
                }

                // Send email
                _logger.LogDebug("Sending email to {Email}", toEmail);
                await client.SendAsync(message);

                // Disconnect
                _logger.LogDebug("Disconnect from SMTP server");
                await client.DisconnectAsync(true);
            }

            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}: {ErrorMessage}", toEmail, ex.Message);
            throw;
        }
    }
}
