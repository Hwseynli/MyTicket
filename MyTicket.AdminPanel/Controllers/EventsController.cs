using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Admin.Event.Create;

namespace MyTicket.AdminPanel.Controllers;
[Route("api/events")]
[ApiController]
[Authorize(Roles = "Admin")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-events")]
    public async Task<IActionResult> CreateEvent([FromForm]CreateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : BadRequest();
    }
}