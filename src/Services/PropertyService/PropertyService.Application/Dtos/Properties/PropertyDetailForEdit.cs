using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Commons;
using Common.Domain.Enums;
using Common.Application.Responses;

namespace PropertyService.Application.Dtos.Properties;

public sealed class PropertyDetailForEdit : IRequest<ApiResponse>
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ListingType ListingType { get; set; }
    public RentPeriod? RentPeriod { get; set; }
    public double Area { get; set; }
    public double LandArea { get; set; }
    public DateTime BuildDate { get; set; }
    public double Price { get; set; }
    public Guid PropertyTypeId { get; set; }
    public required List<string> Images { get; set; }

    [ModelBinder(BinderType = typeof(JsonModelBinder<AddressDto>))]
    public required AddressDto Address { get; set; }

    [ModelBinder(BinderType = typeof(JsonModelBinder<List<RoomDto>>))]
    public List<RoomDto>? Rooms { get; set; }

    [ModelBinder(BinderType = typeof(JsonModelBinder<List<Guid>>))]
    public List<Guid>? UtilityIds { get; set; }
}

public sealed class AddressDto
{
    public required string Country { get; set; }
    public required string Province { get; set; }
    public required string District { get; set; }
    public required string Ward { get; set; }
    public required string Details { get; set; }
}

public sealed class RoomDto
{
    public Guid Id { get; set; }
    public int Quantity { get; set; }
}