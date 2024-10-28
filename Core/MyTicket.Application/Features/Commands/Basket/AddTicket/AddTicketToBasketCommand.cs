using MediatR;

namespace MyTicket.Application.Features.Commands.Basket.AddTicket;
public class AddTicketToBasketCommand : IRequest<bool>
{
    public int TicketId { get; set; }
}
