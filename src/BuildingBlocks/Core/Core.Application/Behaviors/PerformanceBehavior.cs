/*
 * PerformanceBehavior in MediatR Pipeline
 * ---------------------------------------
 * 1. Purpose:
 *    - Monitors the execution time of every request (Command/Query).
 *    - Helps detect slow-running operations.
 *    - Provides fine-grained visibility for performance tuning.
 *
 * 2. Workflow:
 *    - Start a Stopwatch before executing request.
 *    - Call the next() delegate → execute next behavior or handler.
 *    - Stop the Stopwatch when done.
 *    - Measure elapsed milliseconds.
 *    - If execution > 500ms → log a warning with request name and payload.
 *    - Otherwise → log at Debug level for observability.
 *
 * 3. Benefits:
 *    - Early detection of performance bottlenecks in handlers, DB calls, or external APIs.
 *    - Prevents "hidden slow queries" from silently degrading performance.
 *    - Keeps logging noise low (only logs warnings for slow requests, debug for normal).
 *
 * 4. Integration:
 *    - Register in DI (Startup/Program.cs):
 *          services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
 *    - Works together with LoggingBehavior & ValidationBehavior.
 *
 * 5. Example Log Output:
 * ----------------------
 * DEBUG Request: GetUserQuery (43 milliseconds)
 * WARN  Long Running Request: CreateInvoiceCommand (857 milliseconds)
 *       Payload: { UserId = 123, Amount = 9999, Currency = "USD" }
 *
 * 6. Best Practices:
 *    - Adjust threshold (500ms) depending on SLA/requirements.
 *    - Consider tagging requests with correlation IDs for tracing across services.
 *    - In production, you may integrate with APM tools (e.g., OpenTelemetry, Application Insights).
 */

using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Application.Behaviors;

/// <summary>
/// Performance monitoring behavior for MediatR pipeline
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponse">Response type</typeparam>
public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly Stopwatch _stopwatch;

    /// <summary>
    /// Initializes a new instance of PerformanceBehavior
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
        _stopwatch = new Stopwatch();
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _stopwatch.Start();

        var response = await next();

        _stopwatch.Stop();

        var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
        var requestName = typeof(TRequest).Name;

        if (elapsedMilliseconds > 500) // Log if request takes more than 500ms
        {
            _logger.LogWarning(
                "Long Running Request: {RequestName} ({ElapsedMilliseconds} milliseconds) with {@Request}",
                requestName,
                elapsedMilliseconds,
                request);
        }
        else
        {
            _logger.LogDebug(
                "Request: {RequestName} ({ElapsedMilliseconds} milliseconds)",
                requestName,
                elapsedMilliseconds);
        }

        return response;
    }
}
