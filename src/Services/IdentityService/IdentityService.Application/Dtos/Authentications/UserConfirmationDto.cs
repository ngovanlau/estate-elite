namespace IdentityService.Application.Dtos.Authentications;

public class UserConfirmationDto(Guid userId, string confirmationCode, TimeSpan expiryTime, int attemptCount)
{
    public Guid UserId { get; private set; } = userId;
    public string ConfirmationCode { get; private set; } = confirmationCode;
    public DateTime ExpiryDate { get; private set; } = DateTime.UtcNow.Add(expiryTime);
    public int AttemptCount { get; set; } = attemptCount;
}
