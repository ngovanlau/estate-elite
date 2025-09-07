using AutoMapper;
using PropertyService.Application.Dtos.Utilities;
using PropertyService.Domain.Entities;

namespace PropertyService.Infrastructure.Mappers;

public class UtilityMapperProfile : Profile
{
    public UtilityMapperProfile()
    {
        CreateMap<Utility, UtilityDto>();
    }
}
