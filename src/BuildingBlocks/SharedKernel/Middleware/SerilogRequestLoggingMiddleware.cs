using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace SharedKernel.Middleware;

public class SerilogRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogRequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        // Push additional information into the diagnostic context for Serilog
        using (LogContext.PushProperty("RequestHost", httpContext.Request.Host.Value))
        using (LogContext.PushProperty("UserAgent", httpContext.Request.Headers["User-Agent"]))
        using (LogContext.PushProperty("RemoteIp", httpContext.Connection.RemoteIpAddress?.ToString()))
        using (LogContext.PushProperty("TraceId", httpContext.TraceIdentifier))
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                LogContext.PushProperty("UserId", httpContext.User.Identity.Name);
            }

            // Log the request start
            Log.Information("HTTP {RequestMethod} {RequestPath} started", httpContext.Request.Method, httpContext.Request.Path);
            Log.Information("RequestHost: {RequestHost}");
            Log.Information("UserAgent: {UserAgent}");
            Log.Information("RemoteIp: {RemoteIp}");
            Log.Information("TraceId: {TraceId}");
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                Log.Information("UserId: {UserId}");
            }

            // Process the request
            var startTime = DateTime.UtcNow;
            await _next(httpContext);
            var elapsed = DateTime.UtcNow - startTime;

            // Log the response with enriched properties
            Log.Information("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms",
                httpContext.Request.Method,
                httpContext.Request.Path,
                httpContext.Response.StatusCode,
                elapsed.TotalMilliseconds);
        }
    }

}
