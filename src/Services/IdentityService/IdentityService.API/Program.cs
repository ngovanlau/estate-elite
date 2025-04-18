using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using IdentityService.Application.Mediators;
using IdentityService.Application.Validates;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedKernel.Extensions;
using SharedKernel.Middleware;
using SharedKernel.Settings;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Serilog configuration
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext());

// Configurations
// Configures ConfirmationCodeSettings by binding the "ConfirmationCode" section
// from configuration to the ConfirmationCodeSetting class.
// This enables dependency injection via:
// - IOptions<ConfirmationCodeSetting> (singleton, no reload)
// - IOptionsSnapshot<ConfirmationCodeSetting> (scoped, reload per request)
// - IOptionsMonitor<ConfirmationCodeSetting> (singleton, supports reload)
//
// Usage example:
// public SomeService(IOptions<ConfirmationCodeSetting> settings)
// {
//    var codeLength = settings.Value.Length;
// }
builder.Services.Configure<ConfirmationCodeSetting>(configuration.GetSection("ConfirmationCode"));
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add services to the container.
// Mediator
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

    // Add mediator
    configuration.AddAuthenticatorMediator();
    configuration.AddUserMediator();
});
builder.Services.AddValidation();

// Controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddOpenApiService();
builder.Services.AddHealthChecks();

// Add services
builder.Services.AddDistributedService(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddAuthenticationService(configuration);
builder.Services.AddMinioService(configuration);

// Register Event Bus and dependencies
builder.Services.AddEventBusServices(configuration);

// CORS
builder.Services.AddCorsService();

// Rate limit
builder.Services.AddRateLimiterService();

var app = builder.Build();

// Add Serilog request logging
app.UseMiddleware<SerilogRequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API v1");
});

app.UseCors("AllowSpecificOrigin");
app.UseRateLimiter();

// app.UseHttpsRedirection(); enable when use https
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting IdentityService microservice");

    // Apply any pending migrations at startup
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
            Log.Information("Applying database migrations...");
            await dbContext.Database.MigrateAsync(); // Automatically apply migrations
            Log.Information("Database migrations applied successfully");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while applying database migrations");
            throw;
        }
    }

    app.MapHealthChecks("/health");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
