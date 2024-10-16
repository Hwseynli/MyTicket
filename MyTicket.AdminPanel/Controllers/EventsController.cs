using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Event.Create;
using MyTicket.Application.Features.Commands.Event.Update;

namespace MyTicket.AdminPanel.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles ="Admin")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create_events")]
    public async Task<IActionResult> Create([FromForm] CreateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result? Ok():BadRequest("Commandda sehvlik var");
    }

    [HttpPut("update-events")]
    public async Task<IActionResult> Update([FromForm] UpdateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : BadRequest("Commandda sehvlik var");
    }
}

