using AutoMapper;
using IdentityService.Application.Dtos;
using IdentityService.Application.Interfaces;
using IdentityService.Application.Requests;
using IdentityService.Domain.Entities;
using IdentityService.Infrastructure.Data;

namespace IdentityService.Infrastructure.Repositories;

public class UserRepository(IdentityContext context, IMapper mapper) : IUserRepository
{
    public async Task<UserDto> Create(RegisterRequest request)
    {
        var user = User.Create(request.Username + "", request.Email + "", request.Fullname + "", request.Password + "");
        await context.AddAsync(user);
        await context.SaveChangesAsync();

        return mapper.Map<UserDto>(user);
    }
}
