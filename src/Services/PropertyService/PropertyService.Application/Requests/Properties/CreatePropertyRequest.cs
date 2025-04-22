using MediatR;
using System.Text.Json.Serialization;

namespace PropertyService.Application.Requests.Properties;

using Microsoft.AspNetCore.Http;
using SharedKernel.Commons;
using SharedKernel.Converters;
using SharedKernel.Enums;

public sealed record CreatePropertyRequest : IRequest<ApiResponse>
{
    public required string Title { get; init; }
    public required string Description { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ListingType ListingType { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RentPeriod? RentPeriod { get; init; }
    public decimal Area { get; init; }
    public decimal LandArea { get; init; }

    [JsonConverter(typeof(UtcDateTimeConverter))]
    public DateTime BuildDate { get; init; }
    public decimal Price { get; init; }
    public Guid PropertyTypeId { get; init; }
    public required AddressDto Address { get; init; }
    public List<RoomDto>? Rooms { get; init; }
    public List<Guid>? UtilityIds { get; init; }
    public required List<IFormFile> Images { get; init; }
}

public sealed record AddressDto
{
    public required string Country { get; init; }
    public required string Province { get; init; }
    public required string District { get; init; }
    public required string Ward { get; init; }
    public required string Details { get; init; }
}

public sealed record RoomDto
{
    public Guid Id { get; init; }
    public int Quantity { get; init; }
}