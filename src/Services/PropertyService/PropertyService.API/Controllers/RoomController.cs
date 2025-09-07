using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PropertyService.API.Controllers;

using Common.Domain.Constants;
using Common.Presentation.Controllers;
using PropertyService.Application.Requests.Rooms;

public class RoomController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet, Authorize(Policy = Policy.RequireSellerRole)]
    public async Task<IActionResult> GetRoom()
    {
        var request = new GetRoomRequest();
        var response = await _mediator.Send(request);
        return Ok(response);
    }
}
