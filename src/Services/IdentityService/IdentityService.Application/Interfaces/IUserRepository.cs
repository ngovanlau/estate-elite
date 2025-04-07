namespace IdentityService.Application.Interfaces;

using IdentityService.Domain.Entities;

public interface IUserRepository
{
    Task<bool> Add(User user);
    Task<bool> IsUsernameExist(string username);
    Task<bool> IsEmailExist(string email);
}
