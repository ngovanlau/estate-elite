using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.Requests.Properties;
using SharedKernel.Constants;
using SharedKernel.Controllers;

namespace PropertyService.API.Controllers;

public class PropertyController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost, Authorize(Policy = Policy.RequireSellerRole)]
    public async Task<IActionResult> CreateProperty([FromForm] CreatePropertyRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("owner"), Authorize(Policy = Policy.RequireSellerRole)]
    public async Task<IActionResult> GetOwnerProperties()
    {
        var request = new GetOwnerPropertiesRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet(), Authorize]
    public async Task<IActionResult> GetProperties()
    {
        var request = new GetPropertiesRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
