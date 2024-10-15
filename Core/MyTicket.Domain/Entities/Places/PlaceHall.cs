using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Places;
public class PlaceHall : Editable<User>
{
    public string Name { get; private set; }
    public int PlaceId { get; set; }
    public Place Place { get; set; }
    public List<Seat> Seats { get; set; }
    public List<Event> Events { get; set; }

    public void SetDetails(string name, int placeId,int createdById)
    {
        Name = name;
        PlaceId = placeId;
        Seats = new List<Seat>();
        Events = new List<Event>();
        SetAuditDetails(createdById);
    }
    public void SetDetailsFoUpdate(string name, int placeId, int createdById)
    {
        Name = name;
        PlaceId = placeId;
        SetEditFields(createdById);
    }
    public void AddSeat(Seat seat)
    {
        Seats.Add(seat);
    }
}