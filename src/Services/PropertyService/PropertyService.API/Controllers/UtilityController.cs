using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyService.API.Controllers;

using PropertyService.Application.Requests.Utilities;
using Common.Domain.Constants;
using SharedKernel.Controllers;

public class UtilityController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet, Authorize(Policy = Policy.RequireSellerRole)]
    public async Task<IActionResult> GetUtility()
    {
        var request = new GetUtilityRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
