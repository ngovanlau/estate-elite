
using Contracts.Grpc.Protos;
using Grpc.Core;
using IdentityService.Application.Interfaces;

namespace IdentityService.Application.Protos;

public class UserGrpcService(IUserRepository repository) : UserService.UserServiceBase
{
    public override async Task<GetUserResponse?> GetUser(GetUserRequest request, ServerCallContext context)
    {
        var success = Guid.TryParse(request.Id, out var userId);
        if (!success)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user ID format"));
        }

        return await repository.GetDtoByIdAsync<GetUserResponse>(userId, context.CancellationToken);
    }
}
