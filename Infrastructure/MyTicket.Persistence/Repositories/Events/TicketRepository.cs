using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Places;
using MyTicket.Infrastructure.Utils;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Events;
public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context)
    {
    }

    public async Task CreateTickets(List<Seat> seats, decimal eventPrice, int eventId, int userId, CancellationToken cancellationToken)
    {
        foreach (var seat in seats)
        {
            // Yeni bilet yaradılması
            var ticket = new Ticket();
            var uniqueCode = Generator.GenerateUniqueCode();
            if (await IsPropertyUniqueAsync(x => x.UniqueCode, uniqueCode))
            {
                ticket.CreateCalculatePrice(eventPrice, seat.Price);
                ticket.SetTicketDetails(uniqueCode, eventId, seat.Id, userId);
            }
            else
            {
                var uniqueCodeAgain = Generator.GenerateUniqueCode();
                if (await IsPropertyUniqueAsync(x => x.UniqueCode, uniqueCodeAgain))
                {
                    ticket.CreateCalculatePrice(eventPrice, seat.Price);
                    ticket.SetTicketDetails(uniqueCodeAgain, eventId, seat.Id, userId);
                }
            }
            await AddAsync(ticket);
            await Commit(cancellationToken);
        }
    }
}

