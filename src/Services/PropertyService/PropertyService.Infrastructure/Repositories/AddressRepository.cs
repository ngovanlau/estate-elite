using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;

namespace PropertyService.Infrastructure.Repositories;

public class AddressRepository(PropertyContext context) : IAddressRepository
{
    public async Task<bool> AddAddressAsync(Address address, CancellationToken cancellationToken)
    {
        await context.AddAsync(address, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
    }

    public Address Attach(Address entity)
    {
        var entry = context.Addresses.Attach(entity);

        // Mark the entity as modified to ensure changes are saved
        entry.State = EntityState.Modified;

        return entity;
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}