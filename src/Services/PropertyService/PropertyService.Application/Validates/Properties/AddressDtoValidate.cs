using FluentValidation;
using PropertyService.Application.Requests.Properties;

namespace PropertyService.Application.Validates.Properties;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");

        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("Province is required")
            .MaximumLength(100).WithMessage("Province cannot exceed 100 characters");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("District is required")
            .MaximumLength(100).WithMessage("District cannot exceed 100 characters");

        RuleFor(x => x.Ward)
            .NotEmpty().WithMessage("Ward is required")
            .MaximumLength(100).WithMessage("Ward cannot exceed 100 characters");

        RuleFor(x => x.Details)
            .MaximumLength(500).WithMessage("Address details cannot exceed 500 characters");
    }
}
