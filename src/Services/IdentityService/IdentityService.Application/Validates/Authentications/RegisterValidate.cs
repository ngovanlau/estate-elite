using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using Requests.Authentications;
using SharedKernel.Enums;
using SharedKernel.Validators;

public class RegisterValidate : AbstractValidator<RegisterRequest>
{
    public RegisterValidate()
    {
        RuleFor(p => p.Username).Cascade(CascadeMode.Stop).UsernameRule();

        RuleFor(p => p.FullName).Cascade(CascadeMode.Stop)
            .NotEmptyOrWhiteSpaceRule("FullName")
            .MinimumLengthRule(3, "FullName")
            .MaximumLengthRule(30, "FullName");

        RuleFor(p => p.Email).Cascade(CascadeMode.Stop).EmailRule();

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop).PasswordRule();

        RuleFor(p => p.ConfirmationPassword).Cascade(CascadeMode.Stop).PasswordRule();

        RuleFor(p => p.Role)
            .Must(role => role == UserRole.Buyer || role == UserRole.Seller)
            .WithMessage("Role must be either Buyer or Seller.");
    }
}
