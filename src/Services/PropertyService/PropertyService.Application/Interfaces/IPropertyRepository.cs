namespace PropertyService.Application.Interfaces;

using PropertyService.Application.Dtos.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Commons;
using SharedKernel.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<PageResult<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid ownerId, int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    Task<PageResult<PropertyDto>> GetPropertyDtosAsync(int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default);

    Task<PageResult<TDto>> GetPaginatedPropertyDtosAsync<TDto>(
        Func<IQueryable<Property>, IQueryable<Property>> filter,
        int pageSize,
        Guid? lastProperty = null,
        CancellationToken cancellationToken = default) where TDto : class;
}
