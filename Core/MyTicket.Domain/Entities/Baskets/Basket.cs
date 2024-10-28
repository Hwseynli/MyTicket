using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Baskets;
public class Basket:BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; }

    public List<TicketWithTime> TicketsWithTime { get; private set; }

    public void SetDetails(int userId)
    {
        UserId = userId;
        TicketsWithTime = new List<TicketWithTime>();
    }

    public void AddTicket(int ticketId)
    {
        TicketsWithTime.Add(new TicketWithTime(ticketId,Id,DateTime.UtcNow.AddHours(4)));
    }

    public void RemoveTicket(TicketWithTime ticket)
    {
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

