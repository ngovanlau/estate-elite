using EventBus.RabbitMQ.Extensions;
using FunctionalService.Configurations;
using FunctionalService.EventHandlers;
using FunctionalService.Interfaces;
using FunctionalService.Services;
using FunctionalService.Settings;
using Serilog;

// Setup initial logger for startup errors
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting FunctionalService microservice");

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    // Configure logging
    builder.Host.UseSerilog((context, config) => config
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext());

    // API Documentation & Monitoring
    builder.Services.AddOpenApi();
    builder.Services.AddHealthChecks();

    // Event Bus Configuration
    builder.Services.AddEventBusServices(configuration);

    // Configuration Settings
    builder.Services.Configure<SmtpSetting>(configuration.GetSection("SmtpSetting"));

    // Application Services
    builder.Services.AddTransient<IEmailService, EmailService>();

    // Event Handlers
    builder.Services.AddTransient<SendConfirmationCodeEventHandler>();

    var app = builder.Build();

    // Configure event bus subscriptions
    EventBusConfiguration.ConfigureEventBus(app);

    // API Documentation
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    // Health Check Endpoint
    app.MapHealthChecks("/health");

    // HTTPS Redirection (currently disabled)
    // app.UseHttpsRedirection();

    Log.Information("FunctionalService started successfully");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "FunctionalService terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}