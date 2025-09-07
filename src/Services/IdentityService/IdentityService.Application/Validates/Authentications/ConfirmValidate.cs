using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using Requests.Authentications;
using static Common.Domain.Constants.ErrorCode;

public class ConfirmValidate : AbstractValidator<ConfirmRequest>
{
    public ConfirmValidate()
    {
        RuleFor(p => p.UserId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithErrorCode(nameof(E001))
            .WithMessage(string.Format(E001, "UserId"));
    }
}
