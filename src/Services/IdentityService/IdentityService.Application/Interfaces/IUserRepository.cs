namespace IdentityService.Application.Interfaces;

using IdentityService.Domain.Entities;

public interface IUserRepository
{
    Task<bool> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> IsUsernameExistAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken = default);
}
