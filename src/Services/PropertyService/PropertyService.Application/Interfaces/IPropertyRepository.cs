namespace PropertyService.Application.Interfaces;

using PropertyService.Application.Dtos.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<bool> AddProperty(Property property, CancellationToken cancellationToken = default);
    Task<List<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<List<PropertyDto>> GetPropertyDtosAsync(CancellationToken cancellationToken = default);
}
