namespace IdentityService.Application.Dtos.Authentications;

using SharedKernel.Enums;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Fullname { get; set; }
    public required UserRole Role { get; set; }
    public required string PasswordHash { get; set; }
}
