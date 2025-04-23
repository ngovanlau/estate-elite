using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace PropertyService.Application.Queries.Rooms;

using DistributedCache.Redis;
using Interfaces;
using PropertyService.Application.Dtos.Rooms;
using PropertyService.Application.Requests.Rooms;
using SharedKernel.Responses;
using static SharedKernel.Constants.ErrorCode;

public class GetRoomHandler(
    IRoomRepository repository,
    IDistributedCache cache,
    ILogger<GetRoomHandler> logger) : IRequestHandler<GetRoomRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(GetRoomRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();
        logger.LogInformation("Starting to handle GetRoomRequest");

        try
        {
            var cacheKey = CacheKeys.ForDtoCollection<RoomDto>();
            var (success, list) = await cache.TryGetValueAsync<List<RoomDto>>(cacheKey, cancellationToken);
            if (!success || list is null || list.Count == 0)
            {
                logger.LogDebug("Cache miss for key: {CacheKey}", cacheKey);
                logger.LogInformation("Loading property types from repository");
                list = await repository.GetAllRoomDtoAsync(cancellationToken);
                await cache.SetAsync(cacheKey, list, cancellationToken);
            }

            if (list is null || list.Count == 0)
            {
                logger.LogWarning("No Rooms found");
                return res.SetError(nameof(E008), string.Format(E008, "Room list"));
            }

            logger.LogInformation("Successfully retrieved {Count} Rooms", list.Count);
            return res.SetSuccess(list);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving Rooms: {ErrorMessage}", ex.Message);
            return res.SetError(nameof(E000), E000, ex);
        }
    }
}
