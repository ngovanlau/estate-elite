using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
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

    public async Task<List<OwnerPropertyDto>> GetOwnerPropertyDtos(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Available<Property>(false)
            .Where(p => p.OwnerId == userId)
            .ProjectTo<OwnerPropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
