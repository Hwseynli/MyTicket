using MediatR;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
public class RateEventCommand : IRequest<bool>
{
    public int EventId { get; set; } 
    public RatingValue RatingValue { get; set; }
}

