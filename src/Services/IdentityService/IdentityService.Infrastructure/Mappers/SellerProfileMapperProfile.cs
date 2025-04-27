using AutoMapper;

namespace IdentityService.Infrastructure.Mappers;

using Application.Dtos.Users;
using Domain.Entities;
using IdentityService.Application.Requests.Users;

public class SellerProfileMapperProfile : Profile
{
    public SellerProfileMapperProfile()
    {
        CreateMap<SellerProfile, CurrentSellerProfileDto>();
        CreateMap<UpdateSellerProfileRequest, SellerProfile>();
    }
}
