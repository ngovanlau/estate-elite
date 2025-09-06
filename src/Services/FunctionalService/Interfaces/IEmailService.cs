namespace FunctionalService.Interfaces;

public interface IEmailService
{
    Task SendConfirmationCodeAsync(string email, string fullname, string confirmCode, TimeSpan expiryTime);
}
