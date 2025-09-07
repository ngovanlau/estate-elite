/*
 * Why this ExceptionHandlingBehavior is designed this way:
 * ---------------------------------------------------------
 * 1. Purpose:
 *    - Centralizes exception handling for all MediatR requests in the pipeline.
 *    - Converts exceptions into domain-friendly Result<T> objects instead of letting them propagate.
 *    - Ensures consistent error reporting, logging, and mapping to appropriate error types.
 *
 * 2. Logging:
 *    - Each exception is logged with a unique request ID and request type.
 *    - Facilitates diagnostics, debugging, and auditing without exposing sensitive details to clients.
 *
 * 3. Exception Mapping:
 *    - Specific application exceptions (ValidationException, NotFoundException, ConflictException, ForbiddenException) are mapped to corresponding `Error` objects.
 *    - Other ApplicationExceptions fall back to a generic Error with type `Failure`.
 *    - Unknown exceptions produce a standardized unhandled failure result.
 *
 * 4. Generic Result Handling:
 *    - Uses Result / Result<T> pattern to unify success/failure responses.
 *    - Creates failure results dynamically using reflection to support both `Result` and `Result<T>` types.
 *
 * 5. Benefits:
 *    - Reduces boilerplate try/catch in handlers.
 *    - Provides consistent error codes, messages, and types across the application.
 *    - Compatible with domain-driven design and clean architecture principles.
 *
 * 6. Best Practices:
 *    - Keep pipeline behaviors like this at the outermost layer of MediatR.
 *    - Throw typed ApplicationExceptions from handlers for predictable error handling.
 *    - Combine with LoggingBehavior and ValidationBehavior for a robust request pipeline.
 */

using Core.Application.Exceptions;
using Core.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Behaviors;

/// <summary>
/// Exception handling behavior for MediatR pipeline
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponse">Response type</typeparam>
public sealed class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of ExceptionHandlingBehavior
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid();

        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Exception occurred for request {RequestId} {RequestName}: {ExceptionType} - {Message}",
                requestId,
                requestName,
                ex.GetType().Name,
                ex.Message);

            return HandleException<TResponse>(ex);
        }
    }

    /// <summary>
    /// Handles specific exception types and converts to Result
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="exception">Exception to handle</param>
    /// <returns>Result with appropriate error</returns>
    private static TResult HandleException<TResult>(Exception exception)
        where TResult : Result
    {
        var error = exception switch
        {
            ValidationException validationEx => Error.Validation(
                validationEx.ErrorCode,
                validationEx.Message,
                validationEx.Details),

            NotFoundException notFoundEx => Error.NotFound(
                notFoundEx.ErrorCode,
                notFoundEx.Message,
                notFoundEx.Details),

            ConflictException conflictEx => Error.Conflict(
                conflictEx.ErrorCode,
                conflictEx.Message,
                conflictEx.Details),

            UnauthorizedException unauthorizedEx => Error.Unauthorized(
                unauthorizedEx.ErrorCode,
                unauthorizedEx.Message,
                unauthorizedEx.Details),

            ForbiddenException forbiddenEx => Error.Forbidden(
                forbiddenEx.ErrorCode,
                forbiddenEx.Message,
                forbiddenEx.Details),

            _ => new Error(
                "UnhandledException",
                "An unexpected error occurred.")
        };

        return CreateFailureResult<TResult>(error);
    }

    /// <summary>
    /// Creates failure result based on response type
    /// </summary>
    /// <typeparam name="TResult">Result type</typeparam>
    /// <param name="error">Error information</param>
    /// <returns>Failure result</returns>
    private static TResult CreateFailureResult<TResult>(Error error)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (Result.Failure(error) as TResult)!;
        }

        object failureResult = typeof(Result<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(Result.Failure))!
            .Invoke(null, [error])!;

        return (TResult)failureResult;
    }
}
