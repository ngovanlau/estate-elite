using EventBus.RabbitMQ.Extensions;
using FunctionalService.Configurations;
using FunctionalService.EventHandlers;
using FunctionalService.Interfaces;
using FunctionalService.Services;
using FunctionalService.Settings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Serilog configuration
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext());

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(Log.Logger);

// Register Event Bus and dependencies
builder.Services.AddEventBusServices(builder.Configuration);

builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("SmtpSetting"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<SendConfirmationCodeEventHandler>();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure event bus subscriptions
EventBusConfiguration.ConfigureEventBus(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.Run();
