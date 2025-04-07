using MediatR;

namespace IdentityService.Application.Commands.Authentications;

using SharedKernel.Commons;
using Requests.Authentications;
using Validates.Authentications;
using Interfaces;
using SharedKernel.Constants;
using SharedKernel.Extensions;
using static SharedKernel.Constants.ErrorCode;
using Microsoft.Extensions.Caching.Distributed;
using DistributedCache.Redis;
using IdentityService.Domain.Entities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using IdentityService.Application.Dtos.Authentications;

public class ConfirmHandler(
    IUserRepository userRepository,
    IDistributedCache cache) : IRequestHandler<ConfirmRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(ConfirmRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        var validate = new ConfirmValidate().Validate(request);
        if (!validate.IsValid)
        {
            var errors = validate.Errors;
            return res.SetError(nameof(E000), E000, errors);
        }

        var userKey = CacheKeys.ForEntity<User>(request.UserId);
        if (!cache.TryGetValue<User>(userKey, out var user))
        {
            return res.SetError(nameof(E103), E103);
        }

        if (user!.IsActive)
        {
            return res.SetError(nameof(E106), E106);
        }

        var codeKey = CacheKeys.ForDto<UserConfirmationDto>(request.UserId);
        if (!cache.TryGetValue<UserConfirmationDto>(codeKey, out var code)
            || code!.ExpiryDate < DateTime.UtcNow)
        {
            return res.SetError(nameof(E104), E104);
        }

        if (code.AttemptCount == 0)
        {
            return res.SetError(nameof(E108), E108);
        }

        if (code.ConfirmationCode != request.Code)
        {
            return res.SetError(nameof(E105), string.Format(E105, code.AttemptCount), code.AttemptCount);
        }

        user.IsActive = true;
        if (!await userRepository.Add(user))
        {
            return res.SetError(nameof(E107), E107);
        }

        await cache.RemoveAsync(codeKey, cancellationToken);
        await cache.SetAsync(userKey, user);

        return res.SetSuccess(true);
    }
}
