using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Events;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Events;
public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(AppDbContext context) : base(context)
    {
    }
}

