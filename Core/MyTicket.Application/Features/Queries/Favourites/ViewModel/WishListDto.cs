namespace MyTicket.Application.Features.Queries.Favourites.ViewModel;
public class WishListDto
{
    public int UserId { get; set; }
    public List<EventDto> Events { get; set; }
}


