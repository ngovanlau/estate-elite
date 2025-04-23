using System.Text.Json.Serialization;
using SharedKernel.Enums;

namespace PropertyService.Application.Dtos.Properties;

public class OwnerPropertyDto : PropertyDto
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PropertyStatus Status { get; set; }

    [JsonIgnore]
    public override string? ImageUrl { get; set; } = null;
}
