using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace PropertyService.API.Controllers;

using PropertyService.Application.Requests.PropertyTypes;
using SharedKernel.Constants;
using SharedKernel.Controllers;

public class PropertyTypeController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet, Authorize(Policy = Policy.RequireSellerRole)]
    public async Task<IActionResult> GetPropertyType()
    {
        var request = new GetPropertyTypeRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
