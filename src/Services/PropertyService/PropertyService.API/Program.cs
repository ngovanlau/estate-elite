using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Mediators;
using PropertyService.Application.Validates;
using PropertyService.Infrastructure.Data;
using PropertyService.Infrastructure.Extensions;
using Serilog;
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
    builder.Services.AddValidation();

    // Register Event Bus and dependencies
    builder.Services.AddEventBusServices(configuration);

    // Security & Traffic Management
    builder.Services.AddCorsService();
    builder.Services.AddRateLimiterService();

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