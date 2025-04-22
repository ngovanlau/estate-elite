namespace PropertyService.Application.Interfaces;

using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

public interface IAddressRepository : IRepository<Address>
{
    Task<bool> AddAddress(Address address, CancellationToken cancellationToken = default);
}
