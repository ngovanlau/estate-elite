using AutoMapper;

namespace PropertyService.Infrastructure.Mappers;

using Domain.Entities;
using PropertyService.Application.Dtos.Rooms;

public class RoomMapperProfile : Profile
{
    public RoomMapperProfile()
    {
        CreateMap<Room, RoomDto>();
    }
}
