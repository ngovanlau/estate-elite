using SharedKernel.Entities;
using SharedKernel.Enums;

namespace PropertyService.Domain.Entities;

public class Property : AuditableEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ShortDescription { get; set; }
    public ListingType ListingType { get; set; }
    public RentPeriod? RentPeriod { get; set; }
    public decimal Area { get; set; }
    public decimal LandArea { get; set; }
    public DateTime BuildDate { get; set; }
    public decimal Price { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public PropertyStatus Status { get; set; }
    public Guid OwnerId { get; set; }

    public Guid PropertyTypeId { get; set; }
    public required PropertyType Type { get; set; }

    public Guid AddressId { get; set; }
    public required Address Address { get; set; }

    public ICollection<Utility> Utilities { get; set; } = default!;
}
