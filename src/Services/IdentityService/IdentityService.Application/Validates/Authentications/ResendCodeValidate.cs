using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using IdentityService.Application.Requests.Authentications;
using static Common.Domain.Constants.ErrorCode;

public sealed class ResendCodeValidate : AbstractValidator<ResendCodeRequest>
{
    public ResendCodeValidate()
    {
        RuleFor(p => p.UserId).NotEmpty().WithErrorCode(nameof(E001)).WithMessage(string.Format(E001, "UserId"));
    }
}
