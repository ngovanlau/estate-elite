using FluentValidation;

namespace IdentityService.Application.Validates.Authentications;

using Requests.Authentications;
using static SharedKernel.Constants.ErrorCode;

public class RefreshTokenValidate : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidate()
    {
        RuleFor(p => p.RefreshToken).NotEmpty().WithErrorCode(nameof(E008)).WithMessage(string.Format(E008, "RefreshToken"));
    }
}
