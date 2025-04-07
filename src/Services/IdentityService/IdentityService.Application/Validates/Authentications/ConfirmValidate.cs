using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using Requests.Authentications;

public class ConfirmValidate : AbstractValidator<ConfirmRequest>
{
    public ConfirmValidate()
    {
        RuleFor(p => p.UserId)
            .NotEqual(Guid.Empty).WithMessage("User ID cannot be empty.");
    }
}
