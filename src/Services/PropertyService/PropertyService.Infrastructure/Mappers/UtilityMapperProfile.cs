using AutoMapper;

namespace PropertyService.Infrastructure.Mappers;

using Domain.Entities;
using PropertyService.Application.Dtos.Utilities;

public class UtilityMapperProfile : Profile
{
    public UtilityMapperProfile()
    {
        CreateMap<Utility, UtilityDto>();
    }
}
