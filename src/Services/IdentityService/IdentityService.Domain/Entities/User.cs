using Common.Domain.Enums;
using Core.Domain.Primitives;

namespace IdentityService.Domain.Entities;

public partial class User : SoftDeletableEntity
{
    public required string Username { get; init; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public string? Background { get; set; }
    public bool IsActive { get; set; }
    public string? GoogleId { get; set; }

    public SellerProfile? SellerProfile { get; set; }
}
