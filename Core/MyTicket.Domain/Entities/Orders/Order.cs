using MyTicket.Domain.Common;

namespace MyTicket.Domain.Entities.Orders;
public class Order:BaseEntity
{
    public string OrderCode { get; private set; }

}

