using System;
namespace MyTicket.Application.Features.Queries.OrderHistory.ViewModels;
public class TicketViewModel
{
    public string UniqueCode { get; set; }
    public string EventTitle { get; set; }
    public DateTime EventDate { get; set; }
    public int SeatNumber { get; set; }
    public int Rownumber { get; set; }
    public decimal Price { get; set; }
}

