namespace FunctionalService.Configurations;

using EventBus.Infrastructures.Interfaces;
using EventBus.RabbitMQ.Events;
using FunctionalService.EventHandlers;

public class EventBusConfiguration
{
    public static void ConfigureEventBus(WebApplication app)
    {
        var eventBus = app.Services.GetRequiredService<IEventBus>();

        // Subscribe event handlers to event bus
        eventBus.Subscribe<SendConfirmationCodeEvent, SendConfirmationCodeEventHandler>();
    }
}
