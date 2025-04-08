namespace IdentityService.Domain.Entities;

using SharedKernel.Entities;
using SharedKernel.Enums;

public partial class User : AuditableEntity
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Fullname { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public string? Background { get; set; }
    public bool IsActive { get; set; }
}
