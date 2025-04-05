using System;

namespace FunctionalService.Interfaces;

public interface IEmailService
{
    Task SendConfirmationCodeAsync(string email, string username, string confirmCode, int expiryTime);
}
