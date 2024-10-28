using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Domain.Entities.Baskets;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Baskets;
public class TicketWithTimeRepository : Repository<TicketWithTime>, ITicketWithTimeRepository
{
    public TicketWithTimeRepository(AppDbContext context) : base(context)
    {
    }
}

