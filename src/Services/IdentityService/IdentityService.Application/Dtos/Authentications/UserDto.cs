namespace IdentityService.Application.Dtos.Authentications;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Fullname { get; set; }
}
