using AutoMapper;
using PropertyService.Domain.Entities;
using PropertyService.Application.Dtos.Rooms;

namespace PropertyService.Infrastructure.Mappers;

public class RoomMapperProfile : Profile
{
    public RoomMapperProfile()
    {
        CreateMap<Room, RoomDto>();
    }
}
