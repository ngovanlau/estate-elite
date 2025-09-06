namespace IdentityService.Application.Dtos.Authentications;

using Common.Domain.Enums;

public class UserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required UserRole Role { get; set; }
    public required string PasswordHash { get; set; }
}
