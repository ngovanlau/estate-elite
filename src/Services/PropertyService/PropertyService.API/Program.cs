using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Mediators;
using PropertyService.Application.Protos;
using PropertyService.Infrastructure.Data;
using PropertyService.Infrastructure.Extensions;
using Serilog;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using SharedKernel.Middleware;
using System.Reflection;

// Setup initial logger for startup errors
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting PropertyService microservice");

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    var env = builder.Environment;

    // Serilog configuration
    builder.Host.UseSerilog((context, config) => config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext());

    // Add services to the container.
    // Mediator
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        // Add mediator
        config.AddPropertyTypeMediator();
        config.AddUtilityMediator();
        config.AddRoomMediator();
        config.AddPropertyMediator();
    });

    // Controllers
    builder.Services.AddControllers();

    // Documentation & Monitoring
    builder.Services.AddOpenApiService();
    builder.Services.AddHealthChecks();

    // Infrastructure Services
    builder.Services.AddDistributedService(configuration);
    builder.Services.AddAuthenticationService(configuration);
    builder.Services.AddMinioService(configuration);
    builder.Services.AddInfrastructureServices(configuration);
    builder.Services.AddValidation(Assembly.Load("PropertyService.Application"));

    // Register Event Bus and dependencies
    builder.Services.AddEventBusServices(configuration);

    // Security & Traffic Management
    builder.Services.AddCorsService();
    builder.Services.AddRateLimiterService();

    // gRPC Configuration
    builder.Services.AddGrpc(options =>
    {
        options.EnableDetailedErrors = env.IsDevelopment();
        options.Interceptors.Add<GrpcExceptionInterceptor>();
        options.MaxReceiveMessageSize = 16 * 1024 * 1024; // 16 MB
    });

    // Kestrel Configuration
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Explicitly configure endpoints based on configuration
        var kestrelSection = configuration.GetSection("Kestrel:Endpoints");
        if (kestrelSection.Exists())
        {
            options.Configure(kestrelSection);
        }
        else
        {
            // Fallback configuration
            options.ListenAnyIP(5002, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1;
            });

            options.ListenAnyIP(5102, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                listenOptions.UseHttps();
            });
        }
    });

    var app = builder.Build();

    // Middleware Pipeline
    app.UseMiddleware<SerilogRequestLoggingMiddleware>();

    // API Documentation
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Property Service API v1");
    });

    // Security & Traffic Management
    app.UseCors("AllowSpecificOrigin");
    app.UseRateLimiter();
    // app.UseHttpsRedirection(); // Enable when using HTTPS

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    // Endpoints
    app.MapControllers();
    app.MapHealthChecks("/health");

    // gRPC Endpoints
    app.MapGrpcService<PropertyGrpcService>();

    // Apply database migrations and seed data
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PropertyContext>();

    Log.Information("Applying database migrations...");
    await dbContext.Database.MigrateAsync();
    Log.Information("Database migrations applied successfully");

    await DbInitializer.InitializeAsync(dbContext);
    Log.Information("Database seeded successfully");

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}