using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Infrastructure.Extensions;
using Common.Infrastructure.Implements;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Dtos.Utilities;
using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;

namespace PropertyService.Infrastructure.Repositories;

public class UtilityRepository(
    PropertyContext context,
    IMapper mapper) : Repository<Utility>(context, mapper), IUtilityRepository
{
    public async Task<List<UtilityDto>> GetAllUtilityDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<Utility>(false)
            .ProjectTo<UtilityDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Utility>> GetUtilityByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await context.Available<Utility>(false)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }
}
