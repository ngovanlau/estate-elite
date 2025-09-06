using PropertyService.Domain.Entities;
using Common.Application.Interfaces;

namespace PropertyService.Application.Interfaces;

public interface IPropertyViewRepository : IRepository<PropertyView>
{
    Task<bool> HasViewedRecentlyAsync(Guid propertyId, string ipAddress, TimeSpan timeframe);
}
