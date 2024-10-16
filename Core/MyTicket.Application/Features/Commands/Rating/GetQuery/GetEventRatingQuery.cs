using MediatR;

namespace MyTicket.Application.Features.Commands.Rating.GetQuery;
public class GetEventRatingQuery : IRequest<double>
{
    public int EventId { get; set; }
}

