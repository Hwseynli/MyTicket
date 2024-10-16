using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Rating.GetQuery;
using MyTicket.Application.Features.Commands.Rating.RateEvent;

namespace MyTicket.Controllers;
[Route("api/events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Tədbirə reytinq vermək
    [HttpPost("{eventId}/rate")]
    public async Task<IActionResult> RateEvent(RateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Reytinq uğurla əlavə edildi.")
                    : BadRequest("Reytinq əlavə edilərkən xəta baş verdi."); ;
    }

    // Tədbirin ortalama reytinqini almaq
    [HttpGet("{eventId}/average-rating")]
    public async Task<IActionResult> GetAverageRating(GetEventRatingQuery query)
    {
        var averageRating = await _mediator.Send(query);
        return (averageRating >= 0) ? Ok(new { AverageRating = averageRating })
                                  : NotFound("Tədbir tapılmadı.");
    }
}

