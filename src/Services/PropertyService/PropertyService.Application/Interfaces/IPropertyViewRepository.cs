using Common.Application.Interfaces;
using PropertyService.Domain.Entities;

namespace PropertyService.Application.Interfaces;

public interface IPropertyViewRepository : IRepository<PropertyView>
{
    Task<bool> HasViewedRecentlyAsync(Guid propertyId, string ipAddress, TimeSpan timeframe);
}
