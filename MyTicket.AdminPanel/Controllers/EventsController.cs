using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Admin.Event.Create;
using MyTicket.Application.Features.Commands.Admin.Event.Update;
using MyTicket.Application.Features.Queries.Basket;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.AdminPanel.Controllers;
[Route("api/events")]
[ApiController]
[Authorize(Roles = "Admin")]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITicketQueries _ticketQueries;

    public EventsController(IMediator mediator, ITicketQueries ticketQueries)
    {
        _mediator = mediator;
        _ticketQueries = ticketQueries;
    }

    [HttpPost("create-events")]
    public async Task<IActionResult> CreateEvent([FromForm]CreateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("event", "created"))
        : BadRequest(UIMessage.GetFailureMessage("event", "create"));
    }

    [HttpPut("update-events")]
    public async Task<IActionResult> UpdateEvent([FromForm] UpdateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage("event", "updated"))
        : BadRequest(UIMessage.GetFailureMessage("event", "update"));
    }

    [HttpGet("get-reserved-tickets-for-{eventId}")]
    public async Task<IActionResult> GetReservedTicketsForEvent(int eventId)
    {
        var result = await _ticketQueries.GetSoldTicketsForEvent(eventId);
        return Ok(result);
    }
}