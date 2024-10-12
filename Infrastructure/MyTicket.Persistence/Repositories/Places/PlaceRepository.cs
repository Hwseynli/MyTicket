using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Places;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Places;
public class PlaceRepository:Repository<Place>,IPlaceRepository
{
    public PlaceRepository(AppDbContext context) : base(context)
    {
    }
}

