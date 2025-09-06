using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Commons;
using Common.Domain.Enums;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class PropertyRepository(
    PropertyContext context,
    IMapper mapper) : Repository<Property>(context, mapper), IPropertyRepository
{
    private sealed record LastPropertyInfo(Guid Id, DateTime CreatedOn);

    // Offset-base Pagination
    public async Task<PageResult<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid ownerId, int pageSize, int pageNumber, CancellationToken cancellationToken = default)
    {
        var query = context.Available<Property>(false).Where(p => p.OwnerId == ownerId);

        var totalRecords = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(p => p.CreatedOn)
            .Skip(pageNumber - 1)
            .Take(pageSize)
            .ProjectTo<OwnerPropertyDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<OwnerPropertyDto>
        {
            Items = items,
            TotalRecords = totalRecords
        };
    }

    // Key-base Pagination
    public async Task<PageResult<PropertyDto>> GetPropertyDtosAsync(int pageSize, Guid? lastPropertyId = null, Expression<Func<Property, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        return await GetPaginatedPropertyDtosAsync<PropertyDto>(
           query =>
            {
                var filteredQuery = query
                .Include(p => p.Views)
                .Where(p => p.Status == PropertyStatus.Active);
                if (predicate != null)
                {
                    filteredQuery = filteredQuery.Where(predicate);
                }
                return filteredQuery;
            },
           pageSize,
           lastPropertyId,
           cancellationToken);
    }

    public async Task<PageResult<TDto>> GetPaginatedPropertyDtosAsync<TDto>(Func<IQueryable<Property>, IQueryable<Property>> filter, int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default) where TDto : class
    {
        var baseQuery = context.Available<Property>(false).Include(p => p.Address);
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

    public async Task<IEnumerable<PropertyDto>> GetMostViewPropertyDtosAsync(int quantity, CancellationToken cancellationToken = default)
    {
        return await context.Available<Property>(false)
            .Include(p => p.Views)
            .Where(p => p.Status == PropertyStatus.Active)
            .ProjectTo<PropertyDto>(_mapper.ConfigurationProvider)
            .OrderByDescending(p => p.ViewCount)
            .Take(quantity)
            .ToListAsync(cancellationToken);
    }
}
