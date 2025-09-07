using FluentValidation;
using IdentityService.Application.Requests.Users;

namespace IdentityService.Application.Validates.Users;

public class UpdateSellerProfileValidate : AbstractValidator<UpdateSellerProfileRequest>
{
    public UpdateSellerProfileValidate()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters");

        RuleFor(x => x.LicenseNumber)
            .MaximumLength(50).WithMessage("License number cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.LicenseNumber));

        RuleFor(x => x.TaxIdentificationNumber)
            .NotEmpty().WithMessage("Tax identification number is required")
            .MaximumLength(30).WithMessage("Tax identification number cannot exceed 30 characters");

        RuleFor(x => x.ProfessionalLicense)
            .MaximumLength(50).WithMessage("Professional license cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.ProfessionalLicense));

        RuleFor(x => x.Biography)
            .MaximumLength(1000).WithMessage("Biography cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Biography));

        RuleFor(x => x.EstablishedYear)
            .InclusiveBetween(1900, DateTime.Now.Year)
            .WithMessage($"Established year must be between 1900 and {DateTime.Now.Year}");

        RuleFor(x => x.PaypalEmail)
            .EmailAddress().WithMessage("Paypal email must be a valid email address")
            .When(x => !string.IsNullOrEmpty(x.PaypalEmail));

        RuleFor(x => x.PaypalMerchantId)
            .MaximumLength(50).WithMessage("Paypal merchant ID cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.PaypalMerchantId));

        // Additional validation for when AcceptsPaypal is true
        When(x => x.AcceptsPaypal, () =>
        {
            RuleFor(x => x.PaypalEmail)
                .NotEmpty().WithMessage("Paypal email is required when accepting Paypal")
                .EmailAddress().WithMessage("Paypal email must be a valid email address");
        });
    }
}
