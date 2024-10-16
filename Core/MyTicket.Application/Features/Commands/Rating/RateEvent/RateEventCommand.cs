using MediatR;

namespace MyTicket.Application.Features.Commands.Rating.RateEvent;
public class RateEventCommand : IRequest<bool>
{
    public int EventId { get; set; } // Tədbirin ID-si
    public int RatingValue { get; set; } // 1-5 ulduzlu reytinq
}

