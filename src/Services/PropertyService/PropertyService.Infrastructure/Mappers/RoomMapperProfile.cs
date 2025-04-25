using AutoMapper;
using PropertyService.Application.Dtos.Rooms;
using PropertyService.Domain.Entities;

namespace PropertyService.Infrastructure.Mappers;

public class RoomMapperProfile : Profile
{
    public RoomMapperProfile()
    {
        CreateMap<Room, RoomDto>();
    }
}
