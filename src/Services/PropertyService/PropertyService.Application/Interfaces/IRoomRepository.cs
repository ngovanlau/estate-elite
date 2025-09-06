using Microsoft.IdentityModel.Tokens;

namespace PropertyService.Application.Interfaces;

using PropertyService.Application.Dtos.Rooms;
using PropertyService.Domain.Entities;
using Common.Application.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<List<RoomDto>> GetAllRoomDtoAsync(CancellationToken cancellationToken = default);
    Task<Dictionary<Guid, Room>> GetRoomsByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}
