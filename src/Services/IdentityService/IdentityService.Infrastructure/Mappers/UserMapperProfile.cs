using AutoMapper;

namespace IdentityService.Infrastructure.Mappers;

using Application.Dtos.Authentications;
using Application.Dtos.Users;
using Domain.Entities;
using Contracts.Grpc.Protos;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<User, CurrentUserDto>()
            .ForMember(member => member.SellerProfile, opt => opt.MapFrom(src => src.SellerProfile));

        CreateMap<User, GetUserResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone + ""))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.Avatar + ""))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.SellerProfile != null ? src.SellerProfile.CompanyName : ""))
            .ForMember(dest => dest.AcceptsPaypal, opt => opt.MapFrom(src => src.SellerProfile != null ? src.SellerProfile.AcceptsPaypal : false))
            .ForMember(dest => dest.PaypalEmail, opt => opt.MapFrom(src => src.SellerProfile != null ? src.SellerProfile.PaypalEmail + "" : ""))
            .ForMember(dest => dest.PaypalMerchantId, opt => opt.MapFrom(src => src.SellerProfile != null ? src.SellerProfile.PaypalMerchantId + "" : ""));
    }
}
