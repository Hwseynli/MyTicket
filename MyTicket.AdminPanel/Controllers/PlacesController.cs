using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Place.Hall.Create;
using MyTicket.Application.Features.Commands.Place.Hall.Update;
using MyTicket.Application.Features.Commands.Place.Location.Create;
using MyTicket.Application.Features.Commands.Place.Location.Update;
using MyTicket.Application.Features.Commands.Place.Seat.Create;
using MyTicket.Application.Features.Commands.Place.Seat.Update;

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
        return result ? Ok("Place successfully added.") : BadRequest("Failed to add place.");
    }

    [HttpPut("update-places")]
    public async Task<IActionResult> UpdatePlace([FromBody] UpdatePlaceCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Place updated successfully.") : BadRequest("Update failed.");
    }

    // POST: api/places/halls
    [HttpPost("add-halls")]
    public async Task<IActionResult> AddPlaceHall([FromBody] AddPlaceHallCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Place hall successfully added.") : BadRequest("Failed to add place hall.");
    }

    [HttpPut("update-place_halls")]
    public async Task<IActionResult> UpdatePlaceHall([FromBody] UpdatePlaceHallCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("PlaceHall updated successfully.") : BadRequest("Update failed.");
    }

    // POST: api/places/halls/seats
    [HttpPost("add-seats")]
    public async Task<IActionResult> AddSeat([FromBody] AddSeatCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Seat successfully added.") : BadRequest("Failed to add seat.");
    }

    [HttpPut("update-seats")]
    public async Task<IActionResult> Update([FromBody] UpdateSeatCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Seat updated successfully.") : BadRequest("Update failed.");
    }
}