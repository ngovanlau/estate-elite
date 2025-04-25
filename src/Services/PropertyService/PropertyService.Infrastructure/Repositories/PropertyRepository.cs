using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Commons;
using SharedKernel.Enums;
using SharedKernel.Extensions;
using SharedKernel.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class PropertyRepository(PropertyContext context, IMapper mapper) : Repository<Property>(context, mapper), IPropertyRepository
{
    private sealed record LastPropertyInfo(Guid Id, DateTime CreatedOn);

    public async Task<bool> AddProperty(Property property, CancellationToken cancellationToken)
    {
        await context.Properties.AddAsync(property, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
    }

    // Keyset Pagination
    public async Task<PageResult<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid ownerId, int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default)
    {
        return await GetPaginatedPropertyDtosAsync<OwnerPropertyDto>(
            query => query.Where(p => p.OwnerId == ownerId),
            pageSize,
            lastPropertyId,
            cancellationToken);
    }

    // Keyset Pagination
    public async Task<PageResult<PropertyDto>> GetPropertyDtosAsync(int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default)
    {
        return await GetPaginatedPropertyDtosAsync<PropertyDto>(
           query => query.Where(p => p.Status == PropertyStatus.Active),
           pageSize,
           lastPropertyId,
           cancellationToken);
    }

    public async Task<PageResult<TDto>> GetPaginatedPropertyDtosAsync<TDto>(Func<IQueryable<Property>, IQueryable<Property>> filter, int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default) where TDto : class
    {
        var baseQuery = context.Available<Property>(false);
        var query = filter(baseQuery);

        var totalRecords = await query.CountAsync(cancellationToken);

        if (lastPropertyId.HasValue)
        {
            // Áp dụng cursor-based pagination
            if (lastPropertyId.HasValue)
            {
                var lastProperty = await query.Where(p => p.Id == lastPropertyId.Value)
                    .Select(p => new LastPropertyInfo(p.Id, p.CreatedOn))
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastProperty != null)
                {
                    query = query.Where(p => p.CreatedOn < lastProperty.CreatedOn || (p.CreatedOn == lastProperty.CreatedOn && p.Id > lastProperty.Id));
                }
            }
        }

        var items = await query
            .OrderByDescending(p => p.CreatedOn)
            .ThenBy(p => p.Id)  // Đảm bảo thứ tự nhất quán khi CreatedOn giống nhau
            .Take(pageSize)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<TDto>
        {
            Items = items,
            TotalRecords = totalRecords
        };
    }
}
