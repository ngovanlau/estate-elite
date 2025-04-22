using AutoMapper;
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
            .ForMember(dest => dest.Rooms, opt => opt.Ignore());
    }
}