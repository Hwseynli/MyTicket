using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Favourites;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Events;
public class WishListRepository : Repository<WishList>, IWishListRepository
{
    public WishListRepository(AppDbContext context) : base(context)
    {
    }
}

