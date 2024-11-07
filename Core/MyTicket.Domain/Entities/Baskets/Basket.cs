using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Domain.Entities.Baskets;
public class Basket:BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; }

    public List<TicketWithTime>? TicketsWithTime { get; private set; }

    public void SetDetails(int userId)
    {
        UserId = userId;
        TicketsWithTime = new List<TicketWithTime>();
    }

    public void AddTicket(int ticketId)
    {
        TicketsWithTime.Add(new TicketWithTime(ticketId,Id,DateTime.UtcNow.AddHours(4)));
    }

    public void ClearTickets()
    {
        TicketsWithTime?.Clear();
    }

    public void RemoveTicket(int ticketId)
    {
        TicketWithTime? ticket = TicketsWithTime.FirstOrDefault(x=>x.TicketId==ticketId);
        if (ticket == null)
            throw new DomainException(UIMessage.NotFound("Ticket"));

        TicketsWithTime.Remove(ticket);
    }

    // Check if each ticket is expired
    public List<TicketWithTime> GetExpiredTickets()
    {
        var expiredTicketIds = new List<TicketWithTime>();
        var currentTime = DateTime.UtcNow.AddHours(4);

        foreach (var item in TicketsWithTime)
        {
            if (currentTime > item.AddedTime.AddMinutes(15))
                expiredTicketIds.Add(item);
        }

        return expiredTicketIds;
    }
}

