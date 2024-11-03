using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Orders;
public class PromoCodeRepository : Repository<PromoCode>, IPromoCodeRepository
{
    public PromoCodeRepository(AppDbContext context) : base(context)
    {
    }
}

