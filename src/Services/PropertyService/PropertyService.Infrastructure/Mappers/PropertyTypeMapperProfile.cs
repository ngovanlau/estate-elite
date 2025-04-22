using AutoMapper;
using PropertyService.Application.Dtos.PropertyTypes;
using PropertyService.Domain.Entities;

namespace PropertyService.Infrastructure.Mappers;

public class PropertyTypeMapperProfile : Profile
{
    public PropertyTypeMapperProfile()
    {
        CreateMap<PropertyType, PropertyTypeDto>();
    }
}
