using PropertyService.Domain.Entities;
using SharedKernel.Interfaces;

namespace PropertyService.Application.Interfaces;

public interface IPropertyViewRepository : IRepository<PropertyView>
{
    Task<bool> HasViewedRecentlyAsync(Guid propertyId, string ipAddress, TimeSpan timeframe);
}
