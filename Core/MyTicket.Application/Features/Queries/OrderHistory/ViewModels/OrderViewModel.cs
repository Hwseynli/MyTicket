using MyTicket.Domain.Entities.Orders;

namespace MyTicket.Application.Features.Queries.OrderHistory.ViewModels;
public class OrderViewModel
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public List<TicketViewModel> Tickets { get; set; }

    public static OrderViewModel SetDetails(Order order)
    {
       return new OrderViewModel
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            TotalPrice = order.Tickets.Sum(t => t.Price),
            Tickets = SetTicketViewModels(order)
        };
    }

    private static List<TicketViewModel> SetTicketViewModels(Order order)
    {
        return order.Tickets.Select(ticket => new TicketViewModel
        {
            UniqueCode = ticket.UniqueCode,
            EventTitle = ticket.Event.Title,
            EventDate = ticket.Event.StartTime,
            SeatNumber = ticket.Seat.SeatNumber,
            Rownumber = ticket.Seat.RowNumber,
            Price = ticket.Price
        }).ToList();
    }
}

