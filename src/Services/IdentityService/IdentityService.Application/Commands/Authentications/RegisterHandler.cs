using MediatR;

namespace IdentityService.Application.Commands;

using Validates;
using Requests;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using static SharedKernel.Constants.ErrorCode;
using IdentityService.Application.Interfaces;

public class RegisterHandler(IUserRepository repository, IPasswordHasher hasher) : IRequestHandler<RegisterRequest, ApiResponse>
{
    public async Task<ApiResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        var res = new ApiResponse();

        // Validate
        var validate = new RegisterValidate().Validate(request);
        if (!validate.IsValid)
        {
            var errors = validate.Errors.ToDic();
            return res.SetError(nameof(E000), E000, errors);
        }

        request.Password = hasher.Hash(request.Password + "");

        var data = await repository.Create(request);

        return res.SetSuccess(data);
    }
}
