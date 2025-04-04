using IdentityService.Application.Mediators;
using IdentityService.Infrastructure.Data;
using IdentityService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SharedKernel.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Get assembly name
var assembly = Assembly.GetExecutingAssembly().GetName().Name;

var configuration = builder.Configuration;

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
// Mediator
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

    configuration.AddAuthenticatorMediator();
});

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

// Add log service
builder.Services.AddSingleton(Log.Logger);

// Add services from Infrastructure layer
builder.Services.AddInfrastructureServices(configuration);

var app = builder.Build();

// Add Serilog request logging
app.UseMiddleware<SerilogRequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API v1");
});

// app.UseHttpsRedirection(); enable when use https
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
