namespace IdentityService.Application.Dtos.Authentications;

public class UserConfirmationDto(Guid userId, string confirmationCode, TimeSpan expiryTime)
{
    public Guid UserId { get; private set; } = userId;
    public string ConfirmationCode { get; private set; } = confirmationCode;
    public DateTime ExpiryDate { get; private set; } = DateTime.UtcNow.Add(expiryTime);
}
