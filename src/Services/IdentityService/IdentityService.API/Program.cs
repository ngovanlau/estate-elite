﻿using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using IdentityService.Application.Mediators;
using IdentityService.Application.Protos;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.HttpOverrides;
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
    var env = builder.Environment;

    // Configure Serilog
    builder.Host.UseSerilog((context, config) =>
        config.ReadFrom.Configuration(context.Configuration)
              .Enrich.FromLogContext());

    // Configuration Bindings
    builder.Services.Configure<GoogleSetting>(configuration.GetSection("Google"));
    builder.Services.Configure<ConfirmationCodeSetting>(configuration.GetSection("ConfirmationCode"));
    builder.Services.Configure<GrpcEndpointSetting>(configuration.GetSection("GrpcEndpoint"));
    builder.Services.Configure<JsonOptions>(options =>
    {
        options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    // Forwarded Headers Configuration
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders =
            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });

    // Application Services
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        config.AddAuthenticatorMediator();
        config.AddUserMediator();
    });
    builder.Services.AddValidation(Assembly.Load("IdentityService.Application"));
    builder.Services.AddHttpClient();

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
        options.EnableDetailedErrors = env.IsDevelopment();
        options.Interceptors.Add<GrpcExceptionInterceptor>();
    });

    builder.Services.AddDataProtection();

    var app = builder.Build();

    app.UseForwardedHeaders();

    // Configure middleware pipeline
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

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
    app.MapGrpcService<UserGrpcService>();

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