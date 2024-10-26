using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Events;

namespace MyTicket.Domain.Entities.Favourites;
public class WishListEvent : BaseEntity
{
    public int WishListId { get; private set; }
    public WishList WishList { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; }

    public void SetDetails(int wishListId, int eventId)
    {
        WishListId = wishListId;
        EventId = eventId;
    }
}