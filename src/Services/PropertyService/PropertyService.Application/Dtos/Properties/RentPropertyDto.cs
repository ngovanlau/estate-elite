using SharedKernel.Enums;

namespace PropertyService.Application.Dtos.Properties;

public class RentPropertyDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Address { get; set; }
    public ListingType ListingType { get; set; }
    public double Price { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public required string Type { get; set; }
    public double Area { get; set; }
    public RentPeriod? RentPeriod { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}