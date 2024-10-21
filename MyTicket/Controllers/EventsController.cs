using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
using MyTicket.Application.Features.Queries.Event;

namespace MyTicket.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEventQueries _queries;

    public EventsController(IMediator mediator, IEventQueries queries)
    {
        _mediator = mediator;
        _queries = queries;
    }

    // GET: api/values
    [HttpGet("get-events_rating")]
    public async Task<IActionResult> GetRating(int eventId)
    {
        var result=await _queries.GetRating(eventId);
        return Ok(result);
    }

    // POST api/values
    [HttpPost("rate-events")]
    public async Task<IActionResult> RateEvent([FromBody] RateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result? Ok():BadRequest();
    }
}

