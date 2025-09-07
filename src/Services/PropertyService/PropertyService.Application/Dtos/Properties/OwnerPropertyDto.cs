using System.Text.Json.Serialization;
using Common.Domain.Enums;

namespace PropertyService.Application.Dtos.Properties;

public class OwnerPropertyDto : PropertyDto
{
    public PropertyStatus Status { get; set; }

    [JsonIgnore]
    public override string? ImageUrl { get; set; } = null;
}
