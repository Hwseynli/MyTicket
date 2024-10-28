using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Events;

namespace MyTicket.Domain.Entities.Baskets;
public class TicketWithTime:BaseEntity
{
    public int TicketId { get; private set; }
    public Ticket Ticket { get; private set; }
    public DateTime AddedTime { get; private set; }
    public int BasketId { get; private set; }
    public Basket Basket { get; private set; }

    public TicketWithTime(int ticketId, int basketId, DateTime addedTime)
    {
        TicketId = ticketId;
        BasketId = basketId;
        AddedTime = addedTime;
    }
}

