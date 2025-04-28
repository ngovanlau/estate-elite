using FluentValidation;
using PaymentService.Application.Requests;

namespace PaymentService.Application.Validates;

public class CaptureOrderValidate : AbstractValidator<CaptureOrderRequest>
{
    public CaptureOrderValidate()
    {
        RuleFor(x => x.TransactionId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Transaction ID not empty");

        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID is required.");
    }
}
