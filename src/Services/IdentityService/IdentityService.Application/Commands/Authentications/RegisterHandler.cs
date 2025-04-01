using MediatR;

namespace IdentityService.Application.Commands;

using Interfaces;
using Requests;
using SharedKernel.Commons;
using SharedKernel.Extensions;
using Validates;
using static SharedKernel.Constants.ErrorCode;

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

        if (await repository.IsUsernameExist(request.Username + ""))
        {
            return res.SetError(nameof(E101), E101);
        }

        if (await repository.IsEmailExist(request.Email + ""))
        {
            return res.SetError(nameof(E102), E102);
        }

        request.Password = hasher.Hash(request.Password + "");

        var data = await repository.Create(request);

        return res.SetSuccess(data);
    }
}
