using SharedKernel.Enums;
using System.Text.Json.Serialization;

namespace PropertyService.Application.Dtos.Properties;

public class OwnerPropertyDto : PropertyDto
{
    public PropertyStatus Status { get; set; }

    [JsonIgnore]
    public override string? ImageUrl { get; set; } = null;
}
