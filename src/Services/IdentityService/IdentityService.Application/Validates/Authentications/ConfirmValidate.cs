using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using Requests.Authentications;
using static SharedKernel.Constants.ErrorCode;

public class ConfirmValidate : AbstractValidator<ConfirmRequest>
{
    public ConfirmValidate()
    {
        RuleFor(p => p.UserId)
            .NotEmpty()
            .WithErrorCode(nameof(E001))
            .WithMessage(string.Format(E001, "UserId"));
    }
}
