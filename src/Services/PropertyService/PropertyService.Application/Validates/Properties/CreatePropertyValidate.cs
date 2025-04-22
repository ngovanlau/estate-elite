using FluentValidation;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Enums;

namespace PropertyService.Application.Validates.Properties;

public class CreatePropertyRequestValidator : AbstractValidator<CreatePropertyRequest>
{
    public CreatePropertyRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

        RuleFor(x => x.ListingType)
            .IsInEnum().WithMessage("Invalid listing type");

        RuleFor(x => x.RentPeriod)
            .Must((request, rentPeriod) =>
                request.ListingType == ListingType.Rent ? rentPeriod.HasValue : true)
            .WithMessage("Rent period is required for rental properties")
            .When(x => x.ListingType == ListingType.Rent);

        RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("Area must be greater than 0");

        RuleFor(x => x.LandArea)
            .GreaterThanOrEqualTo(0).WithMessage("Land area cannot be negative");

        RuleFor(x => x.BuildDate)
            .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Build date cannot be in the future");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.PropertyTypeId)
            .NotEmpty().WithMessage("Property type is required");

        RuleFor(x => x.Address)
            .NotNull().WithMessage("Address is required")
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.Rooms)
            .ForEach(room => room.SetValidator(new RoomDtoValidator()))
            .When(x => x.Rooms != null && x.Rooms.Any());

        RuleFor(x => x.UtilityIds)
            .NotNull().WithMessage("Utilities list cannot be null")
            .When(x => x.UtilityIds != null && x.UtilityIds.Any());
    }
}