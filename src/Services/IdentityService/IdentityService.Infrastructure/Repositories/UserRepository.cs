using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

using Application.Dtos.Authentications;
using Application.Dtos.Users;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Implements;
using Data;
using Domain.Entities;
using IdentityService.Application.Interfaces;

public class UserRepository(IdentityContext context, IMapper mapper) : Repository<User>(context, mapper), IUserRepository
{
    public async Task<bool> AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.AddAsync(user, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
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

        return await query.ProjectTo<UserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<CurrentUserDto?> GetCurrentUserDtoByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Available<User>(false)
            .Include(u => u.SellerProfile)
            .Where(u => u.Id == userId)
            .ProjectTo<CurrentUserDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
