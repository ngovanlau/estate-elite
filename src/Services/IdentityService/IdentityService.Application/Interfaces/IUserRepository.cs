using IdentityService.Application.Dtos;
using IdentityService.Application.Requests;

namespace IdentityService.Application.Interfaces;

public interface IUserRepository
{
    Task<UserDto> Create(RegisterRequest request);
}
