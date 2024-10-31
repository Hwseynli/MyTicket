using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Domain.Entities.Orders;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Orders;
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PromoCode> GetPromoCodeByIdAsync(int promoCodeId)
    {
        if (promoCodeId <= 0)
            return null;
        var order = await GetAsync(x => x.PromoCodeId == promoCodeId, "PromoCode");
        if (order == null)
            return null;
        var promoCode = order.PromoCode;
        if (promoCode == null)
            return null;
        return promoCode;
    }
}

