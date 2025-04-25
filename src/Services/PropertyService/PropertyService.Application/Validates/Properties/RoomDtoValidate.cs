using FluentValidation;
using PropertyService.Application.Requests.Properties;

namespace PropertyService.Application.Validates.Properties;

public class RoomDtoValidator : AbstractValidator<RoomDto>
{
    public RoomDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Room ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Room quantity must be at least 1");
    }
}