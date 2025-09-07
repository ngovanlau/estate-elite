using Common.Domain.Constants;
using Common.Presentation.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Requests;

namespace PaymentService.API.Controllers;

public class TransactionController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("rent-property"), Authorize(Policy = Policy.RequireBuyerRole)]
    public async Task<IActionResult> RentProperty([FromBody] RentPropertyRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("capture-order"), Authorize(Policy = Policy.RequireBuyerRole)]
    public async Task<IActionResult> CaptureOrder([FromBody] CaptureOrderRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
