using FluentValidation;
using PaymentService.Application.Requests;
using static SharedKernel.Constants.ErrorCode;

namespace PaymentService.Application.Validates;

public class RentPropertyValidate : AbstractValidator<RentPropertyRequest>
{
    public RentPropertyValidate()
    {
        RuleFor(p => p.PropertyId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithErrorCode(E001)
            .WithMessage(string.Format(E001, "Property ID"));

        RuleFor(p => p.RentalPeriod)
            .GreaterThan(0)
            .WithErrorCode(E012)
            .WithMessage(string.Format(E012, "Rental period", 0));
    }
}
