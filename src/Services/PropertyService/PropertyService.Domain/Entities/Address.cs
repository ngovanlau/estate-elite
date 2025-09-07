using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class Address : AuditableEntity
{
    public required string Country { get; set; }
    public required string Province { get; set; }
    public required string District { get; set; }
    public required string Ward { get; set; }
    public string? Details { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? GooglePlaceId { get; set; }
}
