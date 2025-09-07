using AutoMapper;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;

namespace PropertyService.Infrastructure.Mappers;

public class AddressMapperProfile : Profile
{
    public AddressMapperProfile()
    {
        CreateMap<AddressDto, Address>();
    }
}
