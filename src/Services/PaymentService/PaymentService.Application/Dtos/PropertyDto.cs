using System.Text.Json.Serialization;
using SharedKernel.Enums;

namespace PaymentService.Application.Dtos;

public sealed record PropertyDto
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ListingType ListingType { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RentPeriod? RentPeriod { get; set; }

    public double Price { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CurrencyUnit CurrencyUnit { get; set; }

    public Guid OwnerId { get; set; }
}