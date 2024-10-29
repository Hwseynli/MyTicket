using MyTicket.Domain.Entities.Orders;

namespace MyTicket.Application.Interfaces.IManagers;
public interface IPaymentManager
{
    Task<string> ProcessPaymentAsync(Order order);
}

