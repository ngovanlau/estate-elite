using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.Commands.Users;

using FluentValidation;
using Interfaces;
using Requests.Users;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using SharedKernel.Interfaces;
using static SharedKernel.Constants.ErrorCode;

public class UploadHandler(
    ICurrentUserService currentUserService,
    IValidator<UploadRequest> validator,
    IUserRepository userRepository,
    IFileStorageService fileStorageService,
    IDistributedCache cache,
    ILogger<UploadHandler> logger) : IRequestHandler<UploadRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(UploadRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        try
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDic();
                return res.SetError(nameof(E000), E000, errors);
            }

            return res.SetSuccess(true);
        }
        catch (Exception ex)
        {
            return res.SetError(nameof(E000), E000, ex);
        }
    }
}
