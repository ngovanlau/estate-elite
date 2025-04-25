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
    public async Task<IActionResult> GetOwnerProperties([FromQuery] GetOwnerPropertiesRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet(), Authorize]
    public async Task<IActionResult> GetProperties([FromQuery] GetPropertiesRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("{id}"), Authorize]
    public async Task<IActionResult> GetPropertyDetails([FromRoute] Guid id)
    {
        var request = new GetPropertyDetailsRequest
        {
            Id = id
        };
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
