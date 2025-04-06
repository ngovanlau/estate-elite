using AutoMapper;

namespace IdentityService.Infrastructure.Mappers;

using Application.Dtos.Authentications;
using Domain.Entities;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>();
    }
}
