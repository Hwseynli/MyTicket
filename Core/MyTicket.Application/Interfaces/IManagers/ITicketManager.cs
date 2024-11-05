using MyTicket.Domain.Entities.Places;

namespace MyTicket.Application.Interfaces.IManagers;
public interface ITicketManager
{
    Task CreateTickets(List<Seat> seats, decimal eventPrice, int eventId, int userId, CancellationToken cancellationToken);
}

