using System.Reflection;
using IdentityService.Application.Mediators;
using IdentityService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Get assembly name
var me = typeof(Program);
var assembly = me.Assembly.GetName().Name;

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
builder.Services.AddSwaggerGen();

// Add service from Infrastructure layer
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
