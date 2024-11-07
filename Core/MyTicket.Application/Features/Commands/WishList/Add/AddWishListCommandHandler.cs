using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Commands.WishList.Add;
public class AddWishListCommandHandler:IRequestHandler<AddWishListCommand,bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IWishListRepository _wishListRepository;
    private readonly IUserManager _userManager;
    private readonly IDistributedCache _cache;

    public AddWishListCommandHandler(IEventRepository eventRepository, IWishListRepository wishListRepository, IUserManager userManager, IDistributedCache cache)
    {
        _eventRepository = eventRepository;
        _wishListRepository = wishListRepository;
        _userManager = userManager;
        _cache = cache;
    }

    public async Task<bool> Handle(AddWishListCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        string cacheKey = $"wishlist_{userId}";

        var wishList = await _wishListRepository.GetAsync(x => x.UserId == userId, "WishListEvents");

        if (wishList == null)
        {
            wishList = new Domain.Entities.Favourites.WishList();
            wishList.SetDetails(userId);
            await _wishListRepository.AddAsync(wishList);
        }

        // Check if the event is already in the wishlist
        if (wishList.WishListEvents.Any(x => x.EventId == request.EventId))
            throw new BadRequestException("Event is already in the wishlist.");

        // Check for the existence of the event
        var @event = await _eventRepository.GetAsync(e => e.Id == request.EventId && !e.IsDeleted);
        if (@event == null)
            throw new NotFoundException("Tədbir tapılmadı.");

        wishList.AddEventToWishList(@event);

        await _wishListRepository.Update(wishList);
        await _wishListRepository.Commit(cancellationToken);

        // Invalidate the cache to ensure consistency
        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}