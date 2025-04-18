namespace IdentityService.Application.Interfaces;

using Domain.Entities;
using Dtos.Authentications;
using Dtos.Users;

public interface IUserRepository
{
    Task<bool> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameExistAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserDtoByUsernameOrEmailAsync(string? username, string? email, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default);
    Task<CurrentUserDto?> GetCurrentUserDtoByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
