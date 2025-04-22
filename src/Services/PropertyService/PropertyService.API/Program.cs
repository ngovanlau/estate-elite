using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
using Microsoft.EntityFrameworkCore;
using PropertyService.Application.Mediators;
using PropertyService.Infrastructure.Data;
using PropertyService.Infrastructure.Extensions;
using Serilog;
using SharedKernel.Extensions;
using SharedKernel.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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

    // Add mediator
    configuration.AddPropertyTypeMediator();
    configuration.AddUtilityMediator();
    configuration.AddRoomMediator();
    configuration.AddPropertyMediator();
});

// Controllers
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiService();
builder.Services.AddHealthChecks();

// Add services
builder.Services.AddDistributedService(configuration);
builder.Services.AddAuthenticationService(configuration);
builder.Services.AddMinioService(configuration);
builder.Services.AddInfrastructureServices(configuration);

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
            var dbContext = scope.ServiceProvider.GetRequiredService<PropertyContext>();
            Log.Information("Applying database migrations...");
            await dbContext.Database.MigrateAsync(); // Automatically apply migrations
            Log.Information("Database migrations applied successfully");
            await DbInitializer.InitializeAsync(dbContext);
            Log.Information("Database initialize data successfully");
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
