using SharedKernel.Entities;
using Common.Domain.Enums;

namespace PropertyService.Domain.Entities;

public class Property : AuditableEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ListingType ListingType { get; set; }
    public RentPeriod? RentPeriod { get; set; }
    public double Area { get; set; }
    public double LandArea { get; set; }
    public DateTime BuildDate { get; set; }
    public double Price { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public PropertyStatus Status { get; set; }
    public Guid OwnerId { get; set; }

    public Guid PropertyTypeId { get; set; }
    public required PropertyType Type { get; set; }

    public Guid AddressId { get; set; }
    public required Address Address { get; set; }

    public ICollection<PropertyRoom> PropertyRooms { get; set; } = [];
    public ICollection<Utility> Utilities { get; set; } = [];
    public ICollection<Image> Images { get; set; } = [];
    public ICollection<PropertyView> Views { get; set; } = [];
}
