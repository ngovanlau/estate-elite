using Common.Domain.Enums;
using System.Text.Json.Serialization;

namespace PropertyService.Application.Dtos.Properties;

public class PropertyDetailsDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Address { get; set; }
    public ListingType ListingType { get; set; }
    public double Price { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public required string Type { get; set; }
    public double Area { get; set; }
    public List<string> Images { get; set; } = [];
    public List<RoomDetailsDto> Rooms { get; set; } = [];
    public List<string> Utilities { get; set; } = [];
    public string? Description { get; set; }
    public DateTime BuildDate { get; set; }

    [JsonIgnore]
    public Guid OwnerId { get; set; }
    public required OwnerDto Owner { get; set; }
}

public sealed record OwnerDto
{
    public Guid Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? Avatar { get; set; }
    public string? Phone { get; set; }
    public string? CompanyName { get; set; }
    public bool AcceptsPaypal { get; set; }
}

public sealed record RoomDetailsDto
{
    public required string Name { get; set; }
    public int Quantity { get; set; }
}