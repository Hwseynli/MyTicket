using MyTicket.Application.Features.Queries.Ticket.ViewModels;

namespace MyTicket.Application.Features.Queries.Basket;
public interface ITicketQueries
{
    Task<List<TicketDto>> GetTicketsInBasket();
    Task<List<TicketDto>> GetAllTicketsForEvent(int eventId);
    Task<List<TicketDto>> GetSoldTicketsForEvent(int eventId);
    Task<List<TicketDto>> GetReservedTicketsForEvent(int eventId);
}

