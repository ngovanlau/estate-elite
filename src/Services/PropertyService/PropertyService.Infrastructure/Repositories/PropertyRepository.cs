using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Dtos.Properties;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
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

    public async Task<List<OwnerPropertyDto>> GetOwnerPropertyDtosAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Available<Property>(false)
            .Include(p => p.Address)
            .Where(p => p.OwnerId == userId)
            .ProjectTo<OwnerPropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<List<PropertyDto>> GetPropertyDtosAsync(CancellationToken cancellationToken = default)
    {
        return context.Available<Property>(false)
            .Where(p => p.Status == PropertyStatus.Active)
            .OrderByDescending(p => p.CreatedOn)
            .ProjectTo<PropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
