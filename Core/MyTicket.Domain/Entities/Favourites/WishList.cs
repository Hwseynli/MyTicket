using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Favourites;
public class WishList : BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; }
    public List<WishListEvent> WishListEvents { get; private set; }

    public void SetDetails(int userId)
    {
        UserId = userId;
        WishListEvents = new List<WishListEvent>();
    }

    public void AddEventToWishList(Event @event)
    {
        var listEvent = new WishListEvent();
        listEvent.SetDetails(Id, @event.Id);
        WishListEvents.Add(listEvent);
    }

    public void RemoveEventFromWishList(WishListEvent wishListEvent)
    {
        WishListEvents.Remove(wishListEvent);
    }
}

