using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Favourites.ViewModel;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;

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
        int userId = await _userManager.GetCurrentUserId();
        string cacheKey = $"wishlist_{userId}";

        var cachedWishList = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedWishList))
        {
            try
            {
                return JsonSerializer.Deserialize<WishListDto>(cachedWishList);
            }
            catch (JsonException)
            {
                // Log deserialization error and continue to fetch from the repository
            }
        }

        var wishList = await _wishListRepository.GetAsync(x => x.UserId == userId, "WishListEvents.Event");
        if (wishList == null)
        {
            throw new NotFoundException("User's wishlist not found.");
        }

        var wishListDto = new WishListDto
        {
            UserId = userId,
            Events = wishList.WishListEvents.Select(x => new EventDto
            {
                Id = x.Event.Id,
                EventName = x.Event.Title
            }).ToList()
        };

        var serializedWishList = JsonSerializer.Serialize(wishListDto);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60)
        };

        await _cache.SetStringAsync(cacheKey, serializedWishList, options);

        return wishListDto;
    }
}

