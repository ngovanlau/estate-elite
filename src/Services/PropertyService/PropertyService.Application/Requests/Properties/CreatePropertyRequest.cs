using MediatR;

namespace PropertyService.Application.Requests.Properties;

using Common.Application.Responses;
using Common.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Commons;

public sealed record CreatePropertyRequest : IRequest<ApiResponse>
{
    public required string Title { get; init; }
    public required string Description { get; init; }
    public ListingType ListingType { get; init; }
    public RentPeriod? RentPeriod { get; init; }
    public double Area { get; init; }
    public double LandArea { get; init; }
    public DateTime BuildDate { get; init; }
    public double Price { get; init; }
    public Guid PropertyTypeId { get; init; }
    public required List<IFormFile> Images { get; init; }

    [ModelBinder(BinderType = typeof(JsonModelBinder<AddressDto>))]
    public required AddressDto Address { get; init; }

    [ModelBinder(BinderType = typeof(JsonModelBinder<List<RoomDto>>))]
    public List<RoomDto>? Rooms { get; init; }

    [ModelBinder(BinderType = typeof(JsonModelBinder<List<Guid>>))]
    public List<Guid>? UtilityIds { get; init; }
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
