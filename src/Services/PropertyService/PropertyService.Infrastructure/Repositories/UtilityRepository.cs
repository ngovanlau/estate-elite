using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PropertyService.Domain.Entities;
using PropertyService.Infrastructure.Data;
using SharedKernel.Extensions;
using PropertyService.Application.Dtos.Utilities;
using SharedKernel.Implements;
using PropertyService.Application.Interfaces;

namespace PropertyService.Infrastructure.Repositories;

public class UtilityRepository(
    PropertyContext context,
    IMapper mapper) : Repository<Utility>(context), IUtilityRepository
{
    public async Task<List<UtilityDto>> GetAllUtilityDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<Utility>(false)
            .ProjectTo<UtilityDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Utility>> GetUtilityByIdsAsync(List<Guid> ids, CancellationToken cancellationToken)
    {
        return await context.Available<Utility>(false)
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }
}
