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

public class PropertyRepository(PropertyContext context, IMapper mapper) : Repository<Property>(context), IPropertyRepository
{
    public async Task<bool> AddProperty(Property property, CancellationToken cancellationToken)
    {
        await context.Properties.AddAsync(property, cancellationToken);
        return await SaveChangeAsync(cancellationToken);
    }

    // Keyset Pagination
    public async Task<PageResult<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid userId, int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default)
    {
        var query = context.Available<Property>(false)
            .Where(p => p.OwnerId == userId);

        var totalRecords = await query.CountAsync(cancellationToken);

        if (lastPropertyId.HasValue)
        {
            // Áp dụng cursor-based pagination
            if (lastPropertyId.HasValue)
            {
                var lastProperty = await context.Properties
                    .Where(p => p.Id == lastPropertyId.Value && p.OwnerId == userId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastProperty != null)
                {
                    query = query.Where(p => p.CreatedOn < lastProperty.CreatedOn ||
                                         (p.CreatedOn == lastProperty.CreatedOn && p.Id > lastProperty.Id));
                }
            }
        }

        var items = await query
            .OrderByDescending(p => p.CreatedOn)
            .ThenBy(p => p.Id)  // Đảm bảo thứ tự nhất quán khi CreatedOn giống nhau
            .Take(pageSize)
            .ProjectTo<OwnerPropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<OwnerPropertyDto>
        {
            Items = items,
            TotalRecords = totalRecords
        };
    }

    // Keyset Pagination
    public async Task<PageResult<PropertyDto>> GetPropertyDtosAsync(int pageSize, Guid? lastPropertyId = null, CancellationToken cancellationToken = default)
    {
        var query = context.Available<Property>(false)
            .Where(p => p.Status == PropertyStatus.Active);

        var totalRecords = await query.CountAsync(cancellationToken);

        if (lastPropertyId.HasValue)
        {
            // Áp dụng cursor-based pagination
            if (lastPropertyId.HasValue)
            {
                var lastProperty = await context.Properties
                    .Where(p => p.Id == lastPropertyId.Value)
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastProperty != null)
                {
                    query = query.Where(p => p.CreatedOn < lastProperty.CreatedOn ||
                                         (p.CreatedOn == lastProperty.CreatedOn && p.Id > lastProperty.Id));
                }
            }
        }

        var items = await query
            .OrderByDescending(p => p.CreatedOn)
            .ThenBy(p => p.Id)  // Đảm bảo thứ tự nhất quán khi CreatedOn giống nhau
            .Take(pageSize)
            .ProjectTo<PropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<PropertyDto>
        {
            Items = items,
            TotalRecords = totalRecords
        };
    }
}
