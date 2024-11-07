using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Favourites.ViewModel;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Queries.Favourites;
public class FavouriteQueries : IFavouriteQueries
{
    private readonly IDistributedCache _cache;
    private readonly IUserManager _userManager;
    private readonly IWishListRepository _wishListRepository;

    public FavouriteQueries(IDistributedCache cache, IUserManager userManager, IWishListRepository wishListRepository)
    {
        _cache = cache;
        _userManager = userManager;
        _wishListRepository = wishListRepository;
    }

    public async Task<WishListDto> GetWishList()
    {
        var user = await _userManager.GetCurrentUser();
        string cacheKey = $"wishlist_{user.Id}";

        var cachedWishList = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedWishList))
        {
            try
            {
                return JsonSerializer.Deserialize<WishListDto>(cachedWishList);
            }
            catch(JsonException ex)
            {
                throw new JsonException(ex.Message);
            }
        }

        var wishList = await _wishListRepository.GetAsync(x => x.UserId == user.Id, "WishListEvents.Event.EventMedias.Medias", "WishListEvents.Event.PlaceHall.Place", "WishListEvents.Event.Tickets");
        if (wishList == null)
        {
            throw new NotFoundException(UIMessage.NotFound("User's wishlist"));
        }

        var wishListDto = WishListDto.MapToViewModel(wishList);

        var serializedWishList = JsonSerializer.Serialize(wishListDto);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
        };

        await _cache.SetStringAsync(cacheKey, serializedWishList, options);

        return wishListDto;
    }
}