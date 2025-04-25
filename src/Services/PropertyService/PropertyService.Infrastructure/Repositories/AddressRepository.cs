using AutoMapper;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class AddressRepository(PropertyContext context, IMapper mapper) : Repository<Address>(context, mapper), IAddressRepository
{
    public async Task<bool> AddAddressAsync(Address address, CancellationToken cancellationToken)
    {
        await context.AddAsync(address, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
    }
}