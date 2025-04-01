namespace IdentityService.Application.Interfaces;

using Dtos;
using Requests;

public interface IUserRepository
{
    Task<UserDto> Create(RegisterRequest request);
    Task<bool> IsUsernameExist(string username);
    Task<bool> IsEmailExist(string email);
}
