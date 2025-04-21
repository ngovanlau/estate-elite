using AutoMapper;

namespace PropertyService.Infrastructure.Mappers;

using Application.Dtos.PropertyTypes;
using Domain.Entities;

public class PropertyTypeMapperProfile : Profile
{
    public PropertyTypeMapperProfile()
    {
        CreateMap<PropertyType, PropertyTypeDto>();
    }
}
