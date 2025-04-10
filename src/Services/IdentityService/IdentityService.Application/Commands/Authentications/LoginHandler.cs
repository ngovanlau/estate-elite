
using IdentityService.Application.Requests.Authentications;
using MediatR;
using SharedKernel.Commons;

namespace IdentityService.Application.Commands.Authentications;

public class LoginHandler : IRequestHandler<LoginRequest, ApiResponse>
{
    public Task<ApiResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
