namespace IdentityService.Application.Dtos.Authentications;

public class UserConfirmationDto
{
    public Guid UserId { get; set; }
    public required string ConfirmationCode { get; set; }
    public int AttemptCount { get; set; }
}
