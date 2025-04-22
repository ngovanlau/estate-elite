using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Extensions;
using PropertyService.Application.Dtos.Rooms;
using SharedKernel.Implements;
using PropertyService.Application.Interfaces;

namespace PropertyService.Infrastructure.Repositories;

public class RoomRepository(
    PropertyContext context,
    IMapper mapper) : Repository<Room>(context), IRoomRepository
{
    public async Task<List<RoomDto>> GetAllRoomDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<Room>(false)
            .ProjectTo<RoomDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<Guid, Room>> GetRoomsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await context.Available<Room>(false)
            .Where(p => ids.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);
    }
}
