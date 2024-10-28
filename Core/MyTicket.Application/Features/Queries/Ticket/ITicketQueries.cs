using MyTicket.Application.Features.Queries.Ticket.ViewModels;

namespace MyTicket.Application.Features.Queries.Basket;
public interface ITicketQueries
{
    Task<List<TicketDto>> GetTicketsInBasket();
}

