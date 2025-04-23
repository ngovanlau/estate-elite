namespace IdentityService.Application.Dtos.Users;

using SharedKernel.Commons;
using SharedKernel.Enums;
using System.Text.Json.Serialization;

public class CurrentUserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required UserRole Role { get; set; }

    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public string? Background { get; set; }

    [JsonConverter(typeof(UtcDateTimeConverter))]
    public DateTime CreatedOn { get; set; }

    public CurrentSellerProfileDto? SellerProfile { get; set; }
}
