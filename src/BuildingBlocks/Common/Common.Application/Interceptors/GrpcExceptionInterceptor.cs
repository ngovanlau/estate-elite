using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Common.Application.Interceptors;

public class GrpcExceptionInterceptor : Interceptor
{
    private readonly ILogger<GrpcExceptionInterceptor> _logger;

    public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred");

            var errorDetails = new
            {
                ex.ValidationResult?.MemberNames,
                ex.ValidationResult?.ErrorMessage
            };

            var metadata = new Metadata
            {
                { "ValidationErrors", JsonSerializer.Serialize(errorDetails) }
            };

            throw new RpcException(new Status(StatusCode.InvalidArgument, "Validation failed"), ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during gRPC call");
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred"));
        }
    }
}
