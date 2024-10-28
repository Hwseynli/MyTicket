using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Domain.Entities.Baskets;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Baskets;
public class BasketRepository : Repository<Basket>, IBasketRepository
{
    public BasketRepository(AppDbContext context) : base(context)
    {
    }
}

