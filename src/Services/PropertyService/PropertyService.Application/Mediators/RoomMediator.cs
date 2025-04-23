using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyService.Application.Mediators;

using Queries.Rooms;
using Requests.Rooms;
using SharedKernel.Responses;

public static class RoomMediator
{
    public static void AddRoomMediator(this MediatRServiceConfiguration configuration, ServiceLifetime life = ServiceLifetime.Scoped)
    {
        configuration.AddBehavior<IRequestHandler<GetRoomRequest, ApiResponse>, GetRoomHandler>(life);
    }
}
