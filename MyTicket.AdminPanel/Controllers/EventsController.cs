using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Event.Create;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result? Ok():BadRequest("Commandda sehvlik var");
    }
}

