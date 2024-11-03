using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Application.Interfaces.IRepositories.Events;
public interface ITicketRepository:IRepository<Ticket>
{
    Task CreateTickets(List<Seat> seats, decimal eventPrice, int eventId, int userId, CancellationToken cancellationToken);
}

