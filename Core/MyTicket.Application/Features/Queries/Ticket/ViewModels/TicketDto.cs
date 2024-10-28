namespace MyTicket.Application.Features.Queries.Ticket.ViewModels;
public class TicketDto
{
    public int Id { get; set; }
    public string UniqueCode { get; set; }
    public string EventName { get; set; }
    public string PlaceHallName { get; set; }
    public int SeatNumber { get; set; }
    public int RowNumber { get; set; }
    public decimal Price { get; set; }

    public TicketDto(int id, string uniqueCode, string eventName, string placeHallName, int seatNumber, int rowNumber, decimal price)
    {
        Id = id;
        UniqueCode = uniqueCode;
        EventName = eventName;
        PlaceHallName = placeHallName;
        SeatNumber = seatNumber;
        RowNumber = rowNumber;
        Price = price;
    }
}

