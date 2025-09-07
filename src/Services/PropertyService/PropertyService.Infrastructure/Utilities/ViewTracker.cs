using PropertyService.Application.Interfaces;
using PropertyService.Domain.Entities;

namespace PropertyService.Infrastructure.Utilities;

public class ViewTracker(IPropertyViewRepository repository) : IViewTracker
{
    public async Task TrackViewAsync(Guid propertyId, Guid? userId, string ipAddress, string? userAgent)
    {
        var view = new PropertyView
        {
            PropertyId = propertyId,
            UserId = userId,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await repository.AddEntityAsync(view);
    }
}
