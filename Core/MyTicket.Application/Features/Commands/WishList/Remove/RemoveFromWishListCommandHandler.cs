using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.WishList.Remove;
public class RemoveFromWishListCommandHandler : IRequestHandler<RemoveFromWishListCommand, bool>
{
    private readonly IWishListRepository _wishListRepository;
    private readonly IUserManager _userManager;
    private readonly IDistributedCache _cache;

    public RemoveFromWishListCommandHandler(IWishListRepository wishListRepository, IUserManager userManager, IDistributedCache cache)
    {
        _wishListRepository = wishListRepository;
        _userManager = userManager;
        _cache = cache;
    }

    public async Task<bool> Handle(RemoveFromWishListCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();
        string cacheKey = $"wishlist_{userId}";

        // Retrieve the user's wishlist
        var wishList = await _wishListRepository.GetAsync(x => x.UserId == userId, "WishListEvents.Event");

        if (wishList == null)
            throw new NotFoundException(UIMessage.NotFound("User's wishlist"));

        // Check if the event exists in the wishlist
        var wishListEvent = wishList.WishListEvents.FirstOrDefault(x => x.EventId == request.EventId);
        if (wishListEvent == null)
            throw new BadRequestException(UIMessage.NotFound("Event") +" in wishlist.");

        wishList.RemoveEventFromWishList(wishListEvent);

        await _wishListRepository.Update(wishList);
        await _wishListRepository.Commit(cancellationToken);

        // Invalidate the cache
        await _cache.RemoveAsync(cacheKey);

        return true;
    }
}

