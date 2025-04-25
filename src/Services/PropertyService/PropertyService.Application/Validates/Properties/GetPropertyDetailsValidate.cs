using FluentValidation;
using PropertyService.Application.Requests.Properties;
using static SharedKernel.Constants.ErrorCode;

namespace PropertyService.Application.Validates.Properties;

public class GetPropertyDetailsValidate : AbstractValidator<GetPropertyDetailsRequest>
{
    public GetPropertyDetailsValidate()
    {
        RuleFor(p => p.Id)
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithErrorCode(nameof(E001))
            .WithMessage(string.Format(E001, "Id"));
    }
}
