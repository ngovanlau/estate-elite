using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using IdentityService.Application.Interfaces;
using Requests.Authentications;
using SharedKernel.Enums;

public class RegisterValidate : AbstractValidator<RegisterRequest>
{
    public RegisterValidate()
    {
        RuleFor(p => p.Username)
            .Cascade(CascadeMode.Stop)
            .Matches(@"^[a-zA-Z0-9_]{3,20}$").WithMessage("Username must be between 3 and 20 characters, and can only contain letters, numbers, and underscores.");

        RuleFor(p => p.Email)
            .Cascade(CascadeMode.Stop)
            .EmailAddress().WithMessage("Please enter a valid email address.");

        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(p => p.ConfirmationPassword)
            .Cascade(CascadeMode.Stop)
            .MinimumLength(6).WithMessage("ConfirmationPassword must be at least 6 characters long.")
            .Matches(@"[A-Z]").WithMessage("ConfirmationPassword must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("ConfirmationPassword must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("ConfirmationPassword must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("ConfirmationPassword must contain at least one special character.");

        RuleFor(p => p.Role)
            .Must(role => role == UserRole.Buyer || role == UserRole.Seller)
            .WithMessage("Role must be either Buyer or Seller.");
    }
}
