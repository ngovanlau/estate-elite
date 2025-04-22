using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyService.API.Controllers;

using PropertyService.Application.Requests.Properties;
using PropertyService.Application.Requests.PropertyTypes;
using SharedKernel.Constants;
using SharedKernel.Controllers;

public class PropertyController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost, Authorize(Policy = Policy.RequireSellerRole)]
    public async Task<IActionResult> CreateProperty([FromForm] CreatePropertyRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
