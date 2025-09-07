using System.Linq.Expressions;
using Common.Application.Interfaces;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Domain.Entities;
using SharedKernel.Commons;

namespace PropertyService.Application.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<PageResult<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid ownerId, int pageSize, int pageNumber, CancellationToken cancellationToken = default);
    Task<PageResult<PropertyDto>> GetPropertyDtosAsync(int pageSize, Guid? lastPropertyId = null, Expression<Func<Property, bool>>? predicate = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<PropertyDto>> GetMostViewPropertyDtosAsync(int quantity, CancellationToken cancellationToken = default);

    Task<PageResult<TDto>> GetPaginatedPropertyDtosAsync<TDto>(
        Func<IQueryable<Property>, IQueryable<Property>> filter,
        int pageSize,
        Guid? lastProperty = null,
        CancellationToken cancellationToken = default) where TDto : class;
}
