
using MyTicket.Domain.Entities.Favourites;

namespace MyTicket.Application.Features.Queries.Favourites.ViewModel;
public class WishListDto
{
    public int UserId { get; set; }
    public List<EventDto> Events { get; set; }

    public static WishListDto MapToViewModel(WishList wishList)
    {
        return new WishListDto
        {
            UserId = wishList.UserId,
            Events = wishList.WishListEvents.Select(wishListEvent => EventDto.MapToViewModel(wishListEvent.Event)).ToList() ?? new List<EventDto>()
        };
    }
}


