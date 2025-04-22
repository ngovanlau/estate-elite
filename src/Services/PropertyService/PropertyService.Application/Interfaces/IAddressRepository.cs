using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

namespace PropertyService.Application.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task<bool> AddAddressAsync(Address address, CancellationToken cancellationToken = default);
}
