using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using PropertyService.Application.Interfaces;
using SharedKernel.Extensions;
using PropertyService.Application.Dtos.Rooms;

namespace PropertyService.Infrastructure.Repositories;

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

    public async Task<Room?> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Available<Room>(false).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
