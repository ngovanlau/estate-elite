namespace IdentityService.Application.Interfaces;

public interface IEmailService
{
    Task SendConfirmCodeAsync(string email, string username, string code);
}
