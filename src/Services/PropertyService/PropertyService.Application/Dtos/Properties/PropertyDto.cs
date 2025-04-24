using SharedKernel.Enums;
using System.Text.Json.Serialization;

namespace PropertyService.Application.Dtos.Properties;

public class PropertyDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Address { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ListingType ListingType { get; set; }

    public decimal Price { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CurrencyUnit CurrencyUnit { get; set; }

    public required string Type { get; set; }
    public decimal Area { get; set; }

    [JsonIgnore]
    public string? ObjectName { get; set; }

    public virtual string? ImageUrl { get; set; }
}