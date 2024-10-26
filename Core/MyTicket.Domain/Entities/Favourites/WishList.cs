using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

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

    // Tədbir əlavə etmə metodu
    public void AddEventToWishList(Event @event)
    {
        if (WishListEvents.Any(x => x.EventId == @event.Id))
        {
            throw new DomainException("Bu tədbir artıq WishListdədir.");
        }
        var listEvent = new WishListEvent();
        listEvent.SetDetails(Id, @event.Id);
        WishListEvents.Add(listEvent);
    }

    // Tədbiri çıxarma metodu
    public void RemoveEventFromWishList(int eventId)
    {
        var wishListEvent = WishListEvents.FirstOrDefault(x => x.EventId == eventId);
        if (wishListEvent == null)
        {
            throw new DomainException("Tədbir WishListdə tapılmadı.");
        }

        WishListEvents.Remove(wishListEvent);
    }
}

