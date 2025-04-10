using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using IdentityService.Application.Extensions;
using IdentityService.Application.Mediators;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SharedKernel.Constants;
using SharedKernel.Middleware;
using System.Reflection;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Get assembly name
var assembly = Assembly.GetExecutingAssembly().GetName().Name;

var configuration = builder.Configuration;

// Serilog configuration
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext());

// Add services to the container.
// Mediator
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    configuration.AddAuthenticatorMediator();
});
builder.Services.AddValidation();

// Controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Identity Service API",
        Version = "v1"
    });
});
builder.Services.AddHealthChecks();

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

// Add services
builder.Services.AddDistributedServices(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddAuthenticationService(configuration);

// Register Event Bus and dependencies
builder.Services.AddEventBusServices(configuration);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Rate limit
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("ApiLimit", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? "Anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

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
