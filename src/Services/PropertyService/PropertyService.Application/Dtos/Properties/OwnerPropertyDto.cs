using SharedKernel.Enums;

namespace PropertyService.Application.Dtos.Properties;

public class OwnerPropertyDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Address { get; set; }
    public ListingType ListingType { get; set; }
    public decimal Price { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public PropertyStatus Status { get; set; }
    public required string PropertyType { get; set; }
    public decimal Area { get; set; }
}
