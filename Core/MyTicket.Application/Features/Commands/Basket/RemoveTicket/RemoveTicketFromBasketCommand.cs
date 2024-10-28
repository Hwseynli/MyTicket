using MediatR;

namespace MyTicket.Application.Features.Commands.Basket.RemoveTicket;
public class RemoveTicketFromBasketCommand : IRequest<bool>
{
    public int TicketId { get; set; }
}

