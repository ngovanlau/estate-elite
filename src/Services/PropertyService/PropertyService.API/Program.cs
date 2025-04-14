using DistributedCache.Redis.Extensions;
using EventBus.RabbitMQ.Extensions;
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

app.Run();
