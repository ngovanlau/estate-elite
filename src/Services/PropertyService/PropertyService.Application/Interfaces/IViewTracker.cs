namespace PropertyService.Application.Interfaces;

public interface IViewTracker
{
    Task TrackViewAsync(Guid propertyId, Guid? userId, string ipAddress, string? userAgent);
}
