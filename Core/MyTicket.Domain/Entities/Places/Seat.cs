using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Domain.Entities.Places;
public class Seat : Editable<User>
{
    public int RowNumber { get; private set; }
    public int SeatNumber { get; private set; }
    public SeatType SeatType { get; set; }
    public decimal Price { get; private set; }
    public int PlaceHallId { get; private set; }
    public PlaceHall? PlaceHall { get; private set; }
    public List<Ticket> Tickets { get; private set; }

    public void SetDetail(int rowNumber, int seatNumber, SeatType seatType, decimal price, int userId)
    {
        if (rowNumber <= 0 || seatNumber <= 0)
            throw new DomainException("Sətir və oturacaq nömrələri sıfır və ya mənfi ola bilməz.");
        if (price <= 0)
            throw new DomainException("Qiymət sıfır və ya mənfi ola bilməz.");

        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatType = seatType;
        Price = price;
        Tickets = new List<Ticket>();
        SetAuditDetails(userId);
    }
    public void SetDetailForUpdate(int rowNumber, int seatNumber, SeatType seatType, decimal price, int modifiedById)
    {
        if (rowNumber <= 0 || seatNumber <= 0)
            throw new DomainException("Sətir və oturacaq nömrələri sıfır və ya mənfi ola bilməz.");
        if (price <= 0)
            throw new DomainException("Qiymət sıfır və ya mənfi ola bilməz.");

        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatType = seatType;
        Price = price;
        SetEditFields(modifiedById);
    }

    public SeatType DetermineSeatType(int row, int totalRows)
    {
        // Burada hər sıranın tipini təyin edirik
        if (row <= totalRows / 3)
            return SeatType.FrontRow;
        else if (row > totalRows / 3 && row <= 2 * totalRows / 3)
            return SeatType.MiddleRow;
        else
            return SeatType.BackRow;
    }

    public decimal CalculateSeatPrice(SeatType seatType)
    {
        // Burada sıraya əsasən qiymət hesablanır
        if (seatType == SeatType.FrontRow)
            return 100m; // FrontRow qiyməti
        else if (seatType==SeatType.BackRow)
            return 50m; // BackRow qiyməti
        else
            return 75m; // MiddleRow qiyməti
    }
}

