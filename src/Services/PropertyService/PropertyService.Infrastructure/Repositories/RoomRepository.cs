using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Dtos.Rooms;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class RoomRepository(
    PropertyContext context,
    IMapper mapper) : Repository<Room>(context, mapper), IRoomRepository
{
    public async Task<List<RoomDto>> GetAllRoomDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<Room>(false)
            .ProjectTo<RoomDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<Guid, Room>> GetRoomsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await context.Available<Room>(false)
            .Where(p => ids.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}
