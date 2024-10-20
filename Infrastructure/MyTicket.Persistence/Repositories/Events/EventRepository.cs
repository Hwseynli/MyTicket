using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Events;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Events;
public class EventRepository : Repository<Event>,IEventRepository
{
    public EventRepository(AppDbContext context) : base(context)
    {
    }
}

