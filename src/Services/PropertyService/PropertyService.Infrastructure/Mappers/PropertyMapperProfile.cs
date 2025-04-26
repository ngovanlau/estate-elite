using AutoMapper;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Protos;

namespace PropertyService.Infrastructure.Mappers;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<CreatePropertyRequest, Property>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => PropertyStatus.Active))
            .ForMember(dest => dest.CurrencyUnit, opt => opt.MapFrom(_ => CurrencyUnit.USD))
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.AddressId, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Utilities, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyRooms, opt => opt.Ignore());

        CreateMap<Property, OwnerPropertyDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Details))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name));

        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.ObjectName, opt => opt.MapFrom(src => src.Images.Select(p => p.ObjectName).FirstOrDefault()))
            .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Details))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name));

        CreateMap<Property, PropertyDetailsDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address.Details))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.Name))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(p => p.ObjectName).ToList()))
            .ForMember(dest => dest.Utilities, opt => opt.MapFrom(src => src.Utilities.Select(p => p.Name).ToList()))
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.PropertyRooms.Select(p => new RoomDetailsDto
            {
                Name = p.Room.Name,
                Quantity = p.Quantity
            }).ToList()));

        CreateMap<GetUserResponse, OwnerDto>();

        CreateMap<Property, GetPropertyResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.ListingType, opt => opt.MapFrom(src => src.ListingType.ToString()))
            .ForMember(dest => dest.RentPeriod, opt => opt.MapFrom(src => src.RentPeriod != null ? src.RentPeriod.ToString() : string.Empty))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.ToString("F2")))
            .ForMember(dest => dest.CurrencyUnit, opt => opt.MapFrom(src => src.CurrencyUnit.ToString()))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => src.OwnerId.ToString()));
    }
}