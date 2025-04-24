using AutoMapper;

namespace IdentityService.Infrastructure.Mappers;

using Application.Dtos.Authentications;
using Application.Dtos.Users;
using Domain.Entities;
using SharedKernel.Protos;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<User, CurrentUserDto>()
            .ForMember(member => member.SellerProfile, opt => opt.MapFrom(src => src.SellerProfile));

        CreateMap<User, GetUserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.SellerProfile != null ? src.SellerProfile.CompanyName : ""));
    }
}
