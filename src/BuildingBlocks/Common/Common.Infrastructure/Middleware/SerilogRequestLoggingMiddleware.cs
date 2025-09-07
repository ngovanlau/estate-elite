using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace Common.Infrastructure.Middleware;

public class SerilogRequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogRequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        using (LogContext.PushProperty("RequestHost", httpContext.Request.Host.Value))
        using (LogContext.PushProperty("UserAgent", httpContext.Request.Headers["User-Agent"]))
        using (LogContext.PushProperty("RemoteIp", httpContext.Connection.RemoteIpAddress?.ToString()))
        using (LogContext.PushProperty("TraceId", httpContext.TraceIdentifier))
        {
            if (httpContext.User.Identity?.IsAuthenticated == true)
            {
                LogContext.PushProperty("UserId", httpContext.User.Identity.Name);
            }

            Log.Information("HTTP {RequestMethod} {RequestPath} started",
                httpContext.Request.Method, httpContext.Request.Path);

            var startTime = DateTime.UtcNow;
            try
            {
                await _next(httpContext);
                var elapsed = DateTime.UtcNow - startTime;

                Log.Information("HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds} ms",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    httpContext.Response.StatusCode,
                    elapsed.TotalMilliseconds);
            }
            catch (Exception ex)
            {
                var elapsed = DateTime.UtcNow - startTime;
                Log.Error(ex, "HTTP {RequestMethod} {RequestPath} failed after {ElapsedMilliseconds} ms",
                    httpContext.Request.Method,
                    httpContext.Request.Path,
                    elapsed.TotalMilliseconds);
                throw; // Re-throw to maintain the original behavior
            }
        }
    }
}
