using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using Requests.Authentications;
using SharedKernel.Enums;

public class RegisterValidate : AbstractValidator<RegisterRequest>
{
    public RegisterValidate()
    {
        RuleFor(p => p.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Username is required.")
            .Matches(@"^[a-zA-Z0-9_]{3,20}$").WithMessage("Username must be between 3 and 20 characters, and can only contain letters, numbers, and underscores.");

        RuleFor(p => p.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please enter a valid email address.");

        RuleFor(p => p.Fullname)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Fullname is required.");

        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.");

        RuleFor(p => p.Role)
            .Must(role => role == UserRole.Buyer || role == UserRole.Seller)
            .WithMessage("Role must be either Buyer or Seller.");
    }
}
