using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

using Application.Dtos;
using Application.Interfaces;
using Application.Requests;
using Data;
using Domain.Entities;

public class UserRepository(IdentityContext context, IMapper mapper) : IUserRepository
{
    public async Task<UserDto> Create(RegisterRequest request)
    {
        var user = User.Create(request.Username + "", request.Email + "", request.Fullname + "", request.Password + "");
        await context.AddAsync(user);
        await context.SaveChangesAsync();

        return mapper.Map<UserDto>(user);
    }

    public async Task<bool> IsUsernameExist(string username)
    {
        return await context.Users.AnyAsync(x => x.Username == username);
    }

    public async Task<bool> IsEmailExist(string email)
    {
        return await context.Users.AnyAsync(x => x.Email == email);
    }
}
