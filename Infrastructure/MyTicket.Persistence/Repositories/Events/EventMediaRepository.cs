using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Events;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Events;
public class EventMediaRepository : Repository<EventMedia>, IEventMediaRepository
{
    public EventMediaRepository(AppDbContext context) : base(context)
    {
    }
}

