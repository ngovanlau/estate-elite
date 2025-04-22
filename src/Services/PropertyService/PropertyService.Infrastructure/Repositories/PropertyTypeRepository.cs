using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using PropertyService.Application.Dtos.PropertyTypes;
using SharedKernel.Extensions;
using SharedKernel.Implements;
using PropertyService.Application.Interfaces;

namespace PropertyService.Infrastructure.Repositories;

public class PropertyTypeRepository(
    PropertyContext context,
    IMapper mapper) : Repository<PropertyType>(context), IPropertyTypeRepository
{
    public async Task<List<PropertyTypeDto>> GetAllPropertyTypeDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<PropertyType>(false)
            .ProjectTo<PropertyTypeDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<PropertyType?> GetPropertyTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Available<PropertyType>(false).FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
