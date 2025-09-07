using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using Requests.Authentications;
using static Common.Domain.Constants.ErrorCode;

public class RefreshTokenValidate : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidate()
    {
        RuleFor(p => p.AccessToken).NotEmpty().WithErrorCode(nameof(E001)).WithMessage(string.Format(E001, "AccessToken"));

        RuleFor(p => p.RefreshToken).NotEmpty().WithErrorCode(nameof(E001)).WithMessage(string.Format(E001, "RefreshToken"));
    }
}
