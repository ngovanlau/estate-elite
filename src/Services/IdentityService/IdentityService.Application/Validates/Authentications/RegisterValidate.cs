using FluentValidation;

namespace IdentityService.Application.Validates;

using Requests;

public class RegisterValidate : AbstractValidator<RegisterRequest>
{
    public RegisterValidate()
    {
        RuleFor(p => p.Username).NotEmpty().WithMessage("Username is not empty");
        RuleFor(p => p.Email).NotEmpty().WithMessage("Username is not empty");
        RuleFor(p => p.Fullname).NotEmpty().WithMessage("Username is not empty");
        RuleFor(p => p.Password).NotEmpty().WithMessage("Username is not empty");
    }
}
