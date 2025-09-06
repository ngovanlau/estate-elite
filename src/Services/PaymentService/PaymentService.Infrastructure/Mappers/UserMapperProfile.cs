using AutoMapper;
using PaymentService.Application.Dtos;
using Contracts.Grpc.Protos;

namespace PaymentService.Infrastructure.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<GetUserResponse, SellerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
    }
}