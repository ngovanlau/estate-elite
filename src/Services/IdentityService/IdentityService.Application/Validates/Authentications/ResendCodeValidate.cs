using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Validates.Authentications;

using DistributedCache.Redis;
using IdentityService.Domain.Entities;
using IdentityService.Application.Requests.Authentications;
using static SharedKernel.Constants.ErrorCode;

public sealed class ResendCodeValidate : AbstractValidator<ResendCodeRequest>
{
    public ResendCodeValidate()
    {
        RuleFor(p => p.UserId).NotEmpty().WithErrorCode(nameof(E008)).WithMessage(string.Format(E008, "UserId"));
    }
}