using SharedKernel.Entities;

namespace PropertyService.Domain.Entities;

public class Address : AuditableEntity
{
    public required string Country { get; set; }
    public required string Province { get; set; }
    public required string District { get; set; }
    public required string Ward { get; set; }
    public string? Details { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public string? GooglePlaceId { get; set; }
}
