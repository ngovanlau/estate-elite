using FunctionalService.Interfaces;
using FunctionalService.Services;
using FunctionalService.Settings;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<SmtpSetting>(builder.Configuration.GetSection("SmtpSetting"));
builder.Services.AddTransient<IEmailService, EmailService>();

// Add serilog service
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Functional Service");

app.MapPost("/send-email", async (EmailRequest request, IEmailService emailService) =>
{
    try
    {
        await emailService.SendConfirmationCodeAsync(request.Mail, request.Usernmae, request.Code, request.ExpiryTime);
        return Results.Ok("Email sent successfully.");
    }
    catch (Exception)
    {
        return Results.StatusCode(500);
    }
});

// app.UseHttpsRedirection();

app.Run();

public class EmailRequest
{
    public string Mail { get; set; } = "";
    public string Usernmae { get; set; } = "";

    public string Code { get; set; } = "";

    public int ExpiryTime { get; set; }
}
