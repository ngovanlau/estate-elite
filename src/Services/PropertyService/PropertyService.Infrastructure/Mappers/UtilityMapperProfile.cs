using AutoMapper;
using PropertyService.Domain.Entities;
using PropertyService.Application.Dtos.Utilities;

namespace PropertyService.Infrastructure.Mappers;

public class UtilityMapperProfile : Profile
{
    public UtilityMapperProfile()
    {
        CreateMap<Utility, UtilityDto>();
    }
}
