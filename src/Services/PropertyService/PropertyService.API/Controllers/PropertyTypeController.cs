using Common.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyService.Application.Requests.PropertyTypes;

namespace PropertyService.API.Controllers;

public class PropertyTypeController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet, Authorize]
    public async Task<IActionResult> GetPropertyType()
    {
        var request = new GetPropertyTypeRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
