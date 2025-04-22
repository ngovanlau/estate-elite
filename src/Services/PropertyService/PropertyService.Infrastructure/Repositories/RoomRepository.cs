using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace PropertyService.Infrastructure.Repositories;

using Domain.Entities;
using Data;
using Application.Interfaces;
using SharedKernel.Extensions;
using PropertyService.Application.Dtos.Rooms;

public class RoomRepository(
    PropertyContext context,
    IMapper mapper) : IRoomRepository
{
    public Room Attach(Room entity)
    {
        var entry = context.Rooms.Attach(entity);

        // Mark the entity as modified to ensure changes are saved
        entry.State = EntityState.Modified;

        return entity;
    }

    public async Task<List<RoomDto>> GetAllRoomDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<Room>(false)
            .ProjectTo<RoomDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
