using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Domain.Entities.Orders;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Orders;
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {
    }
}

