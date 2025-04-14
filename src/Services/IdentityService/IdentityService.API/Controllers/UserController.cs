using IdentityService.Application.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Controllers;

namespace IdentityService.API.Controllers;

[Authorize]
public class UserController(IMediator mediator) : BaseController(mediator)
{
    public async Task<IActionResult> UploadAvatar([FromForm] UploadRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
