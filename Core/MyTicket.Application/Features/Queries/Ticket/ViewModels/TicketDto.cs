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
    public bool IsSold { get; set; }
    public bool IsReserved { get; set; }

    public TicketDto(int id, string uniqueCode, string eventName, string placeHallName, int seatNumber, int rowNumber, decimal price, bool isSold, bool isReserved)
    {
        Id = id;
        UniqueCode = uniqueCode;
        EventName = eventName;
        PlaceHallName = placeHallName;
        SeatNumber = seatNumber;
        RowNumber = rowNumber;
        Price = price;
        IsReserved = isReserved;
        IsSold = isSold;
    }
}

