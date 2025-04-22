using Microsoft.IdentityModel.Tokens;

namespace PropertyService.Application.Interfaces;

using PropertyService.Application.Dtos.Rooms;
using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<List<RoomDto>> GetAllRoomDtoAsync(CancellationToken cancellationToken = default);
    Task<Room?> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
