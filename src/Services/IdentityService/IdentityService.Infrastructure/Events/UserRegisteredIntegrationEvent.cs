namespace IdentityService.Infrastructure.Events;

using EventBus.Infrastructures;
using EventBus.Infrastructures.Interfaces;

public class UserRegisteredIntegrationEvent(string userId, string email, string username, string confirmationCode) : IntegrationEvent, IIntegrationEvent
{
    public String UserId { get; private set; } = userId;
    public String Email { get; private set; } = email;
    public string Username { get; private set; } = username;
    public string ConfirmationCode { get; private set; } = confirmationCode;
}
