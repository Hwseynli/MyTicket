using MyTicket.Application.Features.Queries.Favourites.ViewModel;

namespace MyTicket.Application.Features.Queries.Favourites;
public interface IFavouriteQueries
{
    Task<WishListDto> GetWishList();
}

