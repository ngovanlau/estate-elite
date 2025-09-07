using Common.Domain.Constants;
using Common.Presentation.Controllers;
using IdentityService.Application.Requests.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[Authorize]
public class UserController(IMediator mediator) : BaseController(mediator)
{
    [HttpPatch("upload-avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadRequest request)
    {
        request.SetIsAvatar(true);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPatch("upload-background")]
    public async Task<IActionResult> UploadBackground([FromForm] UploadRequest request)
    {
        request.SetIsAvatar(false);
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var request = new CurrentUserRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPatch("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPatch("update-seller-profile"), Authorize(Policy = Policy.RequireSellerRole)]
    public async Task<IActionResult> UpdateSellerProfile([FromBody] UpdateSellerProfileRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
