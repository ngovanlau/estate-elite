using AutoMapper;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Enums;

namespace PropertyService.Application.Mapping;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<CreatePropertyRequest, Property>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => PropertyStatus.Active))
            .ForMember(dest => dest.CurrencyUnit, opt => opt.MapFrom(_ => CurrencyUnit.VND))
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore())
            .ForMember(dest => dest.AddressId, opt => opt.Ignore())
            .ForMember(dest => dest.Type, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Utilities, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyRooms, opt => opt.Ignore());

        CreateMap<Property, OwnerPropertyDto>()
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.Details}, {src.Address.Ward}, {src.Address.District}, {src.Address.Province}, {src.Address.Country}"))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.Type.Name));
    }
}