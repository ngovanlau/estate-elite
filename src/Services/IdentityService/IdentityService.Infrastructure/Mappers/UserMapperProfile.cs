using AutoMapper;

namespace IdentityService.Infrastructure.Mappers;

using Application.Dtos.Authentications;
using Application.Dtos.Users;
using Domain.Entities;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<User, CurrentUserDto>()
            .ForMember(member => member.SellerProfile, opt => opt.MapFrom(src => src.SellerProfile));
    }
}
