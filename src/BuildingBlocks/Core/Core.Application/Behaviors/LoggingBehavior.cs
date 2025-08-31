/*
 * LoggingBehavior in MediatR Pipeline
 * -----------------------------------
 * 1. Purpose:
 *    - Provides centralized logging for every request/response passing through MediatR.
 *    - Tracks request execution time, success, failures, and exceptions.
 *
 * 2. Workflow:
 *    - Generate a RequestId (GUID) for correlation.
 *    - Log when request starts (with name and timestamp).
 *    - Start a Stopwatch to measure duration.
 *    - Call the next() delegate → execute handler or next behavior.
 *    - On completion:
 *        • If success → log info with elapsed time.
 *        • If failure → log warning with error details + elapsed time.
 *    - If an exception occurs → log error with stack trace and duration, then rethrow.
 *
 * 3. Benefits:
 *    - Consistent and structured logging across the entire application.
 *    - Helps in debugging and monitoring performance (slow queries, failed commands, etc.).
 *    - Works seamlessly with the Result pattern (logs Error when IsFailure).
 *
 * 4. Integration:
 *    - Register in DI (before or after ValidationBehavior depending on order you want):
 *          services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
 *    - This ensures every Command/Query is automatically logged.
 *
 * 5. Example Log Output:
 * ----------------------
 * INFO  Starting request 3f2a1c5d CreateUserCommand at 2025-08-31T12:00:00Z
 * INFO  Request 3f2a1c5d CreateUserCommand completed successfully in 132ms
 *
 * or
 *
 * WARN  Request 3f2a1c5d CreateUserCommand completed with error: ValidationError in 58ms
 *
 * or
 *
 * ERROR Request 3f2a1c5d CreateUserCommand failed with exception in 250ms
 *       System.NullReferenceException: Object reference not set...
 */

using System.Diagnostics;
using Core.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Behaviors;

/// <summary>
/// Logging behavior for MediatR pipeline
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponse">Response type</typeparam>
public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Initializes a new instance of LoggingBehavior
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid();

        _logger.LogInformation(
            "Starting request {RequestId} {RequestName} at {DateTime}",
            requestId,
            requestName,
            DateTime.UtcNow);

        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await next();
            stopwatch.Stop();

            if (result.IsSuccess)
            {
                _logger.LogInformation(
                    "Request {RequestId} {RequestName} completed successfully in {ElapsedMilliseconds}ms",
                    requestId,
                    requestName,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogWarning(
                    "Request {RequestId} {RequestName} completed with error: {Error} in {ElapsedMilliseconds}ms",
                    requestId,
                    requestName,
                    result.Error,
                    stopwatch.ElapsedMilliseconds);
            }

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex,
                "Request {RequestId} {RequestName} failed with exception in {ElapsedMilliseconds}ms",
                requestId,
                requestName,
                stopwatch.ElapsedMilliseconds);
            throw;
        }
    }
}