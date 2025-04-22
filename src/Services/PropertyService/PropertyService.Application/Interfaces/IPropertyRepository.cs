namespace PropertyService.Application.Interfaces;

using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<bool> AddProperty(Property property, CancellationToken cancellationToken = default);
}
