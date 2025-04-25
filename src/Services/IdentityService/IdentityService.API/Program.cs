using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using IdentityService.Application.Mediators;
using IdentityService.Application.Protos;
using IdentityService.Application.Validates;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using SharedKernel.Middleware;
using SharedKernel.Settings;
using System.Reflection;
using System.Text.Json.Serialization;

// Setup initial logger for startup errors
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting IdentityService microservice");

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    // Configure Serilog
    builder.Host.UseSerilog((context, config) =>
        config.ReadFrom.Configuration(context.Configuration)
              .Enrich.FromLogContext());

    // Configuration Bindings
    builder.Services.Configure<ConfirmationCodeSetting>(configuration.GetSection("ConfirmationCode"));
    builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // Application Services
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        config.AddAuthenticatorMediator();
        config.AddUserMediator();
    });

    builder.Services.AddValidation();
    builder.Services.AddControllers();
    builder.Services.AddOpenApiService();
    builder.Services.AddHealthChecks();

    // Infrastructure Services
    builder.Services.AddDistributedService(configuration);
    builder.Services.AddInfrastructureServices(configuration);
    builder.Services.AddAuthenticationService(configuration);
    builder.Services.AddMinioService(configuration);

    // Event Bus
    builder.Services.AddEventBusServices(configuration);

    // Cross-cutting Concerns
    builder.Services.AddCorsService();
    builder.Services.AddRateLimiterService();

    // gRPC Configuration
    builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = true;
        options.Interceptors.Add<GrpcExceptionInterceptor>();
    });

    // Kestrel Configuration
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5000, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        });

        options.ListenAnyIP(5101, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            listenOptions.UseHttps();
        });
    });

    var app = builder.Build();

    // gRPC Endpoints
    app.MapGrpcService<UserGrpcService>();

    // Middleware Pipeline
    app.UseMiddleware<SerilogRequestLoggingMiddleware>();

    // API Documentation
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API v1"));

    // Security & Traffic Management
    app.UseCors("AllowSpecificOrigin");
    app.UseRateLimiter();
    // app.UseHttpsRedirection(); // Enable for HTTPS

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Endpoints
    app.MapControllers();
    app.MapHealthChecks("/health");

    // Apply database migrations
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

    Log.Information("Applying database migrations...");
    await dbContext.Database.MigrateAsync();
    Log.Information("Database migrations applied successfully");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}