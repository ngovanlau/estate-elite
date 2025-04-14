using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

using Application.Requests.Users;
using SharedKernel.Controllers;

[Authorize]
public class UserController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadRequest request)
    {
        request.SetIsAvatar(true);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("upload-background")]
    public async Task<IActionResult> UploadBackground([FromForm] UploadRequest request)
    {
        request.SetIsAvatar(false);
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
