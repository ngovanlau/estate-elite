namespace IdentityService.Application.Dtos.Authentications;

public class GoogleUserInfoDTO
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string Name { get; set; }
    public string? Picture { get; set; }
}
