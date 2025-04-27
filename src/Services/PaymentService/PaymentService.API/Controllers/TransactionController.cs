using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Requests;
using SharedKernel.Constants;
using SharedKernel.Controllers;

namespace PaymentService.API.Controllers;

public class TransactionController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost("rent-property"), Authorize(Policy = Policy.RequireBuyerRole)]
    public async Task<IActionResult> RentProperty([FromBody] RentPropertyRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}