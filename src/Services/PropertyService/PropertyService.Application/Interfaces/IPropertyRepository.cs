namespace PropertyService.Application.Interfaces;

using PropertyService.Application.Dtos.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Commons;
using SharedKernel.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<bool> AddProperty(Property property, CancellationToken cancellationToken = default);
    Task<PageResult<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid userId, int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default);
    Task<PageResult<PropertyDto>> GetPropertyDtosAsync(int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default);
}
