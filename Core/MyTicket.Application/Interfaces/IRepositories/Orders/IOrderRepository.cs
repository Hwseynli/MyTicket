using MyTicket.Domain.Entities.Orders;
using MyTicket.Domain.Entities.PromoCodes;

namespace MyTicket.Application.Interfaces.IRepositories.Orders;
public interface IOrderRepository : IRepository<Order>
{
    Task<PromoCode> GetPromoCodeByIdAsync(int promoCodeId);
}

