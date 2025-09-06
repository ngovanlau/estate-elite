using DistributedCache.Redis;
using MediatR;
using Microsoft.Extensions.Logging;
using PropertyService.Application.Interfaces;
using PropertyService.Application.Requests.Properties;
using PropertyService.Domain.Entities;
using Common.Application.Interfaces;
using StackExchange.Redis;

namespace PropertyService.Application.Commands.Properties;

public class TrackViewHandler(
    IPropertyViewRepository repository,
    IViewTracker viewTracker,
    ICurrentUserService currentUserService,
    IConnectionMultiplexer connectionMultiplexer,
    ILogger<TrackViewHandler> logger) : IRequestHandler<TrackViewRequest>
{
    public async Task Handle(TrackViewRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting to track view for property {PropertyId} from IP {IpAddress}",
            request.PropertyId, request.IpAddress);

        var userId = currentUserService.Id;

        try
        {
            logger.LogDebug("Tracking view details - UserId: {UserId}, UserAgent: {UserAgent}",
                userId.ToString() ?? "anonymous", request.UserAgent);

            var hasViewedRecently = await repository.HasViewedRecentlyAsync(
                request.PropertyId,
                request.IpAddress,
                TimeSpan.FromHours(1));

            if (!hasViewedRecently)
            {
                logger.LogInformation("New unique view detected for property {PropertyId}",
                    request.PropertyId);

                await viewTracker.TrackViewAsync(
                    request.PropertyId,
                    userId,
                    request.IpAddress,
                    request.UserAgent);

                await RedisCacheService.ClearCacheByPrefixAsync(connectionMultiplexer, $"{nameof(Property)}:");

                logger.LogInformation("Successfully tracked view for property {PropertyId}",
                    request.PropertyId);
            }
            else
            {
                logger.LogDebug("Duplicate view detected for property {PropertyId} from IP {IpAddress} (within 1 hour)",
                    request.PropertyId, request.IpAddress);
            }


        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error tracking view for property {PropertyId}. IP: {IpAddress}, User: {UserId}. Error: {ErrorMessage}",
                request.PropertyId,
                request.IpAddress,
                userId.ToString() ?? "anonymous",
                ex.Message);

            throw;
        }
    }
}