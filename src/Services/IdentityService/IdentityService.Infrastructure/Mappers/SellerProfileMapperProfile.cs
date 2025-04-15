using AutoMapper;

namespace IdentityService.Infrastructure.Mappers;

using Application.Dtos.Users;
using Domain.Entities;

public class SellerProfileMapperProfile : Profile
{
    public SellerProfileMapperProfile()
    {
        CreateMap<SellerProfile, CurrentSellerProfileDto>();
    }
}
