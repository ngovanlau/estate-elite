using Microsoft.EntityFrameworkCore;

namespace PropertyService.Infrastructure.Repositories;

using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using PropertyService.Infrastructure.Data;

public class PropertyRepository(
    PropertyContext context,
    IMapper mapper) : IPropertyRepository
{
    public Task<bool> AddProperty(Property property, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Property Attach(Property entity)
    {
        var entry = context.Properties.Attach(entity);

        // Mark the entity as modified to ensure changes are saved
        entry.State = EntityState.Modified;

        return entity;
    }

    public async Task<bool> SaveChangeAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
