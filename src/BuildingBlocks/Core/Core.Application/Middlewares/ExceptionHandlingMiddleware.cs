/* 
 * Why this ExceptionHandlingMiddleware is designed this way:
 * ---------------------------------------------------------
 * 1. Global Exception Handling:
 *    - Centralizes error handling instead of scattering try/catch in controllers.
 *    - Ensures consistent error response format across the entire API.
 *    - Improves maintainability by having one single place to manage exception mapping.
 *
 * 2. Middleware Placement in Pipeline:
 *    - Placed early in the ASP.NET Core request pipeline.
 *    - Captures exceptions thrown by subsequent middlewares or controllers.
 *    - Prevents unhandled exceptions from reaching the host and exposing internal details.
 *
 * 3. Logging:
 *    - Uses ILogger<T> to log unhandled exceptions.
 *    - Provides full exception stack traces for observability and debugging.
 *    - Keeps log/response concerns separate (logs are detailed, responses are safe).
 *
 * 4. JSON Error Response:
 *    - Returns structured JSON instead of raw exception messages.
 *    - Protects sensitive internal data from being exposed.
 *    - Standardizes API error contracts for frontend clients.
 *
 * 5. Simplicity:
 *    - Current version always returns HTTP 500 with a generic message.
 *    - Minimal logic keeps it reliable and easy to extend.
 *
 * 6. Extensibility (Next Steps):
 *    - Can be extended to handle domain-specific exceptions (Validation, NotFound, Unauthorized...).
 *    - Can adopt ProblemDetails (RFC 7807) for richer error information.
 *    - Can integrate with correlation IDs or tracing for distributed systems.
 *
 * Overall:
 * This middleware follows best practices for ASP.NET Core:
 * - Separation of concerns (logging vs. response shaping).
 * - Security (no internal details leaked).
 * - Maintainability (single point of exception handling).
 * - Extensibility (can easily add mappings for domain/application errors).
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Core.Application.Middlewares;

/// <summary>
/// Global exception handling middleware
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of ExceptionHandlingMiddleware
    /// </summary>
    /// <param name="next">Next middleware delegate</param>
    /// <param name="logger">Logger instance</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <returns>Task</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Handles exception and writes response
    /// </summary>
    /// <param name="context">HTTP context</param>
    /// <param name="exception">Exception to handle</param>
    /// <returns>Task</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorResponse = new
        {
            Type = "InternalServerError",
            Title = "Internal Server Error",
            Status = 500,
            Detail = "An unexpected error occurred."
        };

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}
