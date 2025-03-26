
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

using SharedKernel.Controllers;
using Application.Requests;

public class AuthenticationController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
