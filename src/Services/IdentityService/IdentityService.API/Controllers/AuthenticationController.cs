
using MediatR;

namespace IdentityService.API.Controllers;

using IdentityService.Application.Requests.Authentications;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Controllers;

public class AuthenticationController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
