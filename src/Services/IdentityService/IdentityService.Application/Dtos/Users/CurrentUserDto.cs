namespace IdentityService.Application.Dtos.Users;

using Common.Domain.Enums;

public class CurrentUserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required UserRole Role { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public string? Background { get; set; }
    public DateTime CreatedOn { get; set; }
    public CurrentSellerProfileDto? SellerProfile { get; set; }
}
