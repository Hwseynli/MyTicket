using MediatR;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
public class RateEventCommand : IRequest<bool>
{
    public int EventId { get; set; } // Tədbirin ID-si
    public RatingValue RatingValue { get; set; } // 1-5 ulduzlu reytinq
}

