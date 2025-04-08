namespace IdentityService.Application.Dtos.Authentications;

public class UserConfirmationDto(Guid userId, string confirmationCode, DateTime expiryDate, int attemptCount)
{
    public Guid UserId { get; private set; } = userId;
    public string ConfirmationCode { get; private set; } = confirmationCode;
    public DateTime ExpiryDate { get; private set; } = expiryDate;
    public int AttemptCount { get; set; } = attemptCount;
}
