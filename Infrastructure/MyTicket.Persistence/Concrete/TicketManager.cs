using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Places;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Persistence.Concrete;
public class TicketManager : ITicketManager
{
    private readonly ITicketRepository _ticketRepository;

    public TicketManager(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task CreateTickets(List<Seat> seats, decimal eventPrice, int eventId, int userId, CancellationToken cancellationToken)
    {
        foreach (var seat in seats)
        {
            var ticket = new Ticket();
            var uniqueCode = Generator.GenerateUniqueCode();
            if (await _ticketRepository.IsPropertyUniqueAsync(x => x.UniqueCode, uniqueCode))
            {
                ticket.CreateCalculatePrice(eventPrice, seat.Price);
                ticket.SetTicketDetails(uniqueCode, eventId, seat.Id, userId);
            }
            else
            {
                var uniqueCodeAgain = Generator.GenerateUniqueCode();
                if (await _ticketRepository.IsPropertyUniqueAsync(x => x.UniqueCode, uniqueCodeAgain))
                {
                    ticket.CreateCalculatePrice(eventPrice, seat.Price);
                    ticket.SetTicketDetails(uniqueCodeAgain, eventId, seat.Id, userId);
                }
            }
            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.Commit(cancellationToken);
        }
    }
}

