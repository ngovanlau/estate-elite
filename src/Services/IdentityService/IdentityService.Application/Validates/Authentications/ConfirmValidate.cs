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
            .WithErrorCode(nameof(E008))
            .WithMessage(string.Format(E008, "UserId"));
    }
}
