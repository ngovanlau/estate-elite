using IdentityService.Domain.Entities;
using IdentityService.Application.Dtos.Authentications;
using IdentityService.Application.Dtos.Users;
using Common.Application.Interfaces;

namespace IdentityService.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<bool> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameExistAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserDtoByUsernameOrEmailAsync(string? username, string? email, CancellationToken cancellationToken = default);
    Task<CurrentUserDto?> GetCurrentUserDtoByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
