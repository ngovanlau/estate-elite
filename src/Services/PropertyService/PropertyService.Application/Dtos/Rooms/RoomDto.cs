using System;

namespace PropertyService.Application.Dtos.Rooms;

public class RoomDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
