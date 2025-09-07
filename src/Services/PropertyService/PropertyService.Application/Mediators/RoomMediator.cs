using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Application.Mediators;

using Common.Application.Responses;
using Queries.Rooms;
using Requests.Rooms;

public static class RoomMediator
{
    public static void AddRoomMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<GetRoomRequest, ApiResponse>, GetRoomHandler>(life);
    }
}
