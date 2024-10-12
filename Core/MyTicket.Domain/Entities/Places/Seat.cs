using MyTicket.Domain.Entities.Enums;
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

    public void SetDetail(int rowNumber, int seatNumber, SeatType seatType, decimal price, int placeHallId)
    {
        if (rowNumber <= 0 || seatNumber <= 0)
            throw new DomainException("Sətir və oturacaq nömrələri sıfır və ya mənfi ola bilməz.");
        if (price <= 0)
            throw new DomainException("Qiymət sıfır və ya mənfi ola bilməz.");

        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatType = seatType;
        Price = price;
        PlaceHallId = placeHallId;
    }
    public void SetDetailForUpdate(int rowNumber, int seatNumber, SeatType seatType, decimal price, int placeHallId)
    {
        if (rowNumber <= 0 || seatNumber <= 0)
            throw new DomainException("Sətir və oturacaq nömrələri sıfır və ya mənfi ola bilməz.");
        if (price <= 0)
            throw new DomainException("Qiymət sıfır və ya mənfi ola bilməz.");

        RowNumber = rowNumber;
        SeatNumber = seatNumber;
        SeatType = seatType;
        Price = price;
        PlaceHallId = placeHallId;
    }
}

