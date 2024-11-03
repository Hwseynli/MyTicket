using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Places;
public class PlaceHall : Editable<User>
{
    public string Name { get; private set; }
    public int PlaceId { get; private set; }
    public Place Place { get; private set; }
    public int SeatCount { get; private set; }
    public int RowCount { get; private set; }
    public List<Seat> Seats { get; private set; }
    public List<Event> Events { get; private set; }

    public void SetDetails(string name, int placeId, int seatCount, int rowCount, int createdById)
    {
        Name = name.Capitalize();
        PlaceId = placeId;
        SeatCount = seatCount;
        RowCount = rowCount;
        Seats = new List<Seat>();
        Events = new List<Event>();
        SetAuditDetails(createdById);
    }
    public void SetDetailsForUpdate(string name, int placeId, int seatCount, int rowCount, int modifiedById)
    {
        Name = name.Capitalize();
        PlaceId = placeId;
        SeatCount = seatCount;
        RowCount = rowCount;
        SetEditFields(modifiedById);
    }

    public void AddSeat(Seat seat)
    {
        Seats.Add(seat);
    }
}