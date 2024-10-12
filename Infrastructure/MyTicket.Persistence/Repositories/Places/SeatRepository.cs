using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Places;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Places;
public class SeatRepository : Repository<Seat>, ISeatRepository
{
    public SeatRepository(AppDbContext context) : base(context)
    {
    }
}

