using AutoMapper;
using PaymentService.Application.Dtos;
using SharedKernel.Protos;

namespace PaymentService.Infrastructure.Mappers;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<GetPropertyResponse, PropertyDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.RentPeriod, opt => opt.MapFrom(src => string.IsNullOrWhiteSpace(src.RentPeriod) ? null : src.RentPeriod))
            .ForMember(dest => dest.OwnerId, opt => opt.MapFrom(src => Guid.Parse(src.OwnerId)));
    }
}