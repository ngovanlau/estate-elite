using EventBus.RabbitMQ.Extensions;
using FunctionalService.Configurations;
using FunctionalService.EventHandlers;
using FunctionalService.Interfaces;
using FunctionalService.Services;
using FunctionalService.Settings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Use Serilog for logging
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext());

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

// Add EventBus & dependencies
builder.Services.AddEventBusServices(configuration);

builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("SmtpSetting"));

// Add Email service
builder.Services.AddTransient<IEmailService, EmailService>();

// Add event handlers
builder.Services.AddTransient<SendConfirmationCodeEventHandler>();

var app = builder.Build();

// Configure event bus subscriptions
EventBusConfiguration.ConfigureEventBus(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHealthChecks("/health");
// app.UseHttpsRedirection();
app.Run();
