using Grpc.Core;
using PropertyService.Application.Interfaces;
using SharedKernel.Protos;

namespace PropertyService.Application.Protos;

public class PropertyGrpcService(IPropertyRepository repository) : SharedKernel.Protos.PropertyService.PropertyServiceBase
{
    public override async Task<GetPropertyResponse?> GetProperty(GetPropertyRequest request, ServerCallContext context)
    {
        var success = Guid.TryParse(request.Id, out var propertyId);
        if (!success)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid user ID format"));
        }

        return await repository.GetDtoByIdAsync<GetPropertyResponse>(propertyId, context.CancellationToken);
    }
}