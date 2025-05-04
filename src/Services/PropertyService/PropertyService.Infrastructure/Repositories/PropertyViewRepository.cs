using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Extensions;
using SharedKernel.Implements;

namespace PropertyService.Infrastructure.Repositories;

public class PropertyViewRepository(PropertyContext context, IMapper mapper) : Repository<PropertyView>(context, mapper), IPropertyViewRepository
{
    public async Task<bool> HasViewedRecentlyAsync(Guid propertyId, string ipAddress, TimeSpan timeframe)
    {
        var cutoff = DateTime.UtcNow - timeframe;
        return await context.Available<PropertyView>(false)
            .AnyAsync(v => v.PropertyId == propertyId &&
                          v.IpAddress == ipAddress &&
                          v.CreatedOn >= cutoff);
    }
}