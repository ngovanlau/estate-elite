using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

using Application.Dtos.Authentications;
using Application.Dtos.Users;
using Application.Interfaces;
using Data;
using Domain.Entities;
using SharedKernel.Extensions;

public class UserRepository(IdentityContext context, IMapper mapper) : IUserRepository
{
    public async Task<bool> AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.AddAsync(user, cancellationToken);
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<bool> IsUsernameExistAsync(string username, CancellationToken cancellationToken)
    {
        return await context.Users.AnyAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Users.AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<UserDto?> GetUserDtoByUsernameOrEmailAsync(
        string? username,
        string? email,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        var query = context.Available<User>(false)
            .Where(u => (!string.IsNullOrWhiteSpace(username) && u.Username == username) ||
                        (!string.IsNullOrWhiteSpace(email) && u.Email == email));

        return await query.ProjectTo<UserDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Available<User>().FirstOrDefaultAsync(p => p.Id == userId, cancellationToken);
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<CurrentUserDto?> GetCurrentUserDtoByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Available<User>(false)
            .Include(u => u.SellerProfile)
            .Where(u => u.Id == userId)
            .ProjectTo<CurrentUserDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
