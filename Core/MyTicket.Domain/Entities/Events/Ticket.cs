using MyTicket.Domain.Entities.Orders;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Domain.Entities.Events;
public class Ticket : Editable<User>
{
    public string UniqueCode { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; }
    public int SeatId { get; private set; }
    public Seat Seat { get; private set; }
    public decimal Price { get; private set; }
    public bool IsReserved { get; private set; }
    public bool IsSold { get; private set; }
    public int? UserId { get; private set; } // Bilet kimin adınadır.
    public User? User { get; private set; }
    public int? OrderId { get; private set; }
    public Order? Order { get; private set; }

    public void SetTicketDetails(string uniqueCode, int eventId, int seatId, decimal price, int userId)
    {
        UniqueCode = uniqueCode;
        EventId = eventId;
        SeatId = seatId;
        Price = price;
        IsReserved = false;
        IsSold = false;
        UserId = null;
        OrderId = null;
        SetAuditDetails(userId);
    }

    public void ReserveTicket(int? userId, bool isReserved=true)
    {
        if (IsSold)
            throw new DomainException("Bu bilet artıq satılıb.");

        IsReserved = isReserved;
        UserId = userId;
    }

    public void SellTicket(int userId)
    {
        //if (IsReserved == false)
        //    throw new DomainException("Bilet əvvəlcədən rezerv edilməlidir.");

        IsSold = true;
        UserId = userId;
        IsReserved = false;
    }
}
