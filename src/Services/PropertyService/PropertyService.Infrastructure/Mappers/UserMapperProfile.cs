using AutoMapper;
using Contracts.Grpc.Protos;
using PropertyService.Application.Dtos.Properties;

namespace PropertyService.Infrastructure.Mappers;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<GetUserResponse, OwnerDto>();
    }
}
