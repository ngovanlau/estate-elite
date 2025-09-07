using AutoMapper;
using Contracts.Grpc.Protos;
using PaymentService.Application.Dtos;

namespace PaymentService.Infrastructure.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<GetUserResponse, SellerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
    }
}
