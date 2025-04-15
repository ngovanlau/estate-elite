namespace IdentityService.Application.Dtos.Users;

using SharedKernel.Enums;

public class CurrentUserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Fullname { get; set; }
    public required UserRole Role { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public string? Background { get; set; }
}
