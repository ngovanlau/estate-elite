using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Mediators;
using PropertyService.Application.Protos;
using PropertyService.Infrastructure.Data;
using PropertyService.Infrastructure.Extensions;
using Serilog;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using SharedKernel.Middleware;
using SharedKernel.Settings;
using System.Reflection;
using System.Security.Authentication;
using System.Text.Json.Serialization;

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

    // Configuration Bindings
    builder.Services.Configure<GrpcEndpointSetting>(configuration.GetSection("GrpcEndpoint"));

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
    builder.Services.AddValidation(Assembly.Load("PropertyService.Application"));

    // Controllers
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.Converters.Add(new UtcDateTimeConverter());
            options.JsonSerializerOptions.Converters.Add(new NullableEnumConverterFactory());
        });

    // Documentation & Monitoring
    builder.Services.AddOpenApiService();
    builder.Services.AddHealthChecks();

    // Infrastructure Services
    builder.Services.AddDistributedService(configuration);
    builder.Services.AddAuthenticationService(configuration);
    builder.Services.AddMinioService(configuration);
    builder.Services.AddInfrastructureServices(configuration);

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

    builder.Services.AddDataProtection();

    var app = builder.Build();

    // Configure middleware pipeline
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        // API Documentation
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API v1"));
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    // Middleware Pipeline
    app.UseMiddleware<SerilogRequestLoggingMiddleware>();

    // Security & Traffic Management
    app.UseCors("AllowAll");
    app.UseRateLimiter();

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