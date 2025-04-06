namespace IdentityService.Application.Interfaces;

using Dtos.Authentications;
using Requests.Authentications;

public interface IUserRepository
{
    Task<UserDto> Create(RegisterRequest request);
    Task<bool> IsUsernameExist(string username);
    Task<bool> IsEmailExist(string email);
}
