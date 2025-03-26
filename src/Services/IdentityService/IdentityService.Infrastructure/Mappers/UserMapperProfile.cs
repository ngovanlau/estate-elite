using AutoMapper;
using IdentityService.Application.Dtos;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>();
    }
}
