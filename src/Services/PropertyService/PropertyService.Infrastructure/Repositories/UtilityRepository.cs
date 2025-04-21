using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace PropertyService.Infrastructure.Repositories;

using Domain.Entities;
using Data;
using Application.Interfaces;
using SharedKernel.Extensions;
using PropertyService.Application.Dtos.Utilities;

public class UtilityRepository(
    PropertyContext context,
    IMapper mapper) : IUtilityRepository
{
    public Utility Attach(Utility entity)
    {
        var entry = context.Utilities.Attach(entity);

        // Mark the entity as modified to ensure changes are saved
        entry.State = EntityState.Modified;

        return entity;
    }

    public async Task<List<UtilityDto>> GetAllUtilityDtoAsync(CancellationToken cancellationToken)
    {
        return await context.Available<Utility>(false)
            .ProjectTo<UtilityDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
