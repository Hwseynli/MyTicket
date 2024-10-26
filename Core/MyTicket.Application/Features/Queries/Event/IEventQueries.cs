using MyTicket.Application.Features.Queries.Event.ViewModels;

namespace MyTicket.Application.Features.Queries.Event;
public interface IEventQueries
{
    Task<double> GetRating(int eventId);
    Task<List<WishListEventDto>> GetWishList();
}

