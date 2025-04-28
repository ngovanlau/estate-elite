using AutoMapper;
using PropertyService.Application.Dtos.Properties;
using SharedKernel.Protos;

namespace PropertyService.Infrastructure.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<GetUserResponse, OwnerDto>();
    }
}
