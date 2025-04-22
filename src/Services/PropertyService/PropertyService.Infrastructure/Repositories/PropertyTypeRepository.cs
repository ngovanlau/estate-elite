using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using PropertyService.Application.Dtos.PropertyTypes;
using PropertyService.Application.Interfaces;
using SharedKernel.Extensions;

namespace PropertyService.Infrastructure.Repositories;

public class PropertyTypeRepository(
    PropertyContext context,
    IMapper mapper) : IPropertyTypeRepository
{
    public PropertyType Attach(PropertyType entity)
    {
        var entry = context.PropertyTypes.Attach(entity);

        // Mark the entity as modified to ensure changes are saved
        entry.State = EntityState.Modified;

        return entity;
    }

    public async Task<List<PropertyTypeDto>> GetAllPropertyTypeDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<PropertyType>(false)
            .ProjectTo<PropertyTypeDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
