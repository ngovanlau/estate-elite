using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Implements;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Dtos.PropertyTypes;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;

namespace PropertyService.Infrastructure.Repositories;

public class PropertyTypeRepository(
    PropertyContext context,
    IMapper mapper) : Repository<PropertyType>(context, mapper), IPropertyTypeRepository
{
    public async Task<List<PropertyTypeDto>> GetAllPropertyTypeDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<PropertyType>(false)
            .ProjectTo<PropertyTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
