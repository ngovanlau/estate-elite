using Common.Domain.Enums;

namespace PaymentService.Application.Dtos;

public sealed record PropertyDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public ListingType ListingType { get; set; }
    public RentPeriod? RentPeriod { get; set; }
    public double Price { get; set; }
    public CurrencyUnit CurrencyUnit { get; set; }
    public Guid OwnerId { get; set; }
}
