using Serilog;
using EventBus.RabbitMQ.Events;
using EventBus.Abstraction.Interfaces;
using FunctionalService.Interfaces;

namespace FunctionalService.EventHandlers;

public class SendConfirmationCodeEventHandler(IEmailService emailHandler) : IEventHandler<SendConfirmationCodeEvent>
{
    private readonly IEmailService _emailHandler = emailHandler;

    public async Task Handle(SendConfirmationCodeEvent @event)
    {
        Log.Logger.Information("Handling send confirmation code event for {Fullname} with email {Email}", @event.Fullname, @event.Email);

        try
        {
            await _emailHandler.SendConfirmationCodeAsync(@event.Email, @event.Fullname, @event.ConfirmationCode, @event.ExpiryTime);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Error processing email confirmation for {Email}: {ErrorMessage}", @event.Email, ex.Message);
            throw;
        }
    }
}
