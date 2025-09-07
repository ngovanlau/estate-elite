using FluentValidation;
using IdentityService.Application.Requests.Authentications;
using static Common.Domain.Constants.ErrorCode;

namespace IdentityService.Application.Validates.Authentications;

public class GoogleAuthValidate : AbstractValidator<GoogleAuthRequest>
{
    public GoogleAuthValidate()
    {
        RuleFor(p => p.IdToken)
            .NotEmpty()
            .WithErrorCode(nameof(E001))
            .WithMessage(string.Format(E001, "ID token"));
    }
}