using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

using Application.Interfaces;
using Data;
using Domain.Entities;

public class UserRepository(IdentityContext context) : IUserRepository
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
}
