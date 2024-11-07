using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Place.Hall.Create;
using MyTicket.Application.Features.Commands.Place.Hall.Update;
using MyTicket.Application.Features.Commands.Place.Location.Create;
using MyTicket.Application.Features.Commands.Place.Location.Update;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.AdminPanel.Controllers;
[ApiController]
[Route("api/places")]
[Authorize(Roles ="Admin")]
public class PlacesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlacesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST: api/places
    [HttpPost("add-places")]
    public async Task<IActionResult> AddPlace([FromBody] AddPlaceCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("place", "added"))
        : BadRequest(UIMessage.GetFailureMessage("place", "add"));
    }

    [HttpPut("update-places")]
    public async Task<IActionResult> UpdatePlace([FromBody] UpdatePlaceCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("place", "updated"))
        : BadRequest(UIMessage.GetFailureMessage("place", "update"));
    }

    // POST: api/places/halls
    [HttpPost("add-halls")]
    public async Task<IActionResult> AddPlaceHall([FromBody] AddPlaceHallCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("place hall", "added"))
        : BadRequest(UIMessage.GetFailureMessage("place hall", "add"));
    }

    [HttpPut("update-place_halls")]
    public async Task<IActionResult> UpdatePlaceHall([FromBody] UpdatePlaceHallCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("place hall", "updated"))
        : BadRequest(UIMessage.GetFailureMessage("place hall", "update"));
    }
}