using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

using Application.Interfaces;
using Data;
using Domain.Entities;

public class UserRepository(IdentityContext context, IMapper mapper) : IUserRepository
{
    public async Task<bool> Add(User user)
    {
        await context.AddAsync(user);
        return await context.SaveChangesAsync() > 0;
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
