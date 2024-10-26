using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Commands.WishList.Add;
public class AddWishListCommandHandler:IRequestHandler<AddWishListCommand,bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IWishListRepository _wishListRepository;
    private readonly IUserManager _userManager;

    public AddWishListCommandHandler(IEventRepository eventRepository, IWishListRepository wishListRepository, IUserManager userManager)
    {
        _eventRepository = eventRepository;
        _wishListRepository = wishListRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(AddWishListCommand request, CancellationToken cancellationToken)
    {
        int userId = _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException();
        // İstifadəçinin mövcud WishListini tapırıq
        var wishList = await _wishListRepository.GetAsync(x => x.UserId == userId, "WishListEvents");

        if (wishList == null)
        {
            // Əgər istifadəçinin WishListi yoxdursa, yeni bir WishList yaradılır
            wishList = new Domain.Entities.Favourites.WishList();
            wishList.SetDetails(userId);
            await _wishListRepository.AddAsync(wishList);
        }

        // Tədbirin mövcudluğunu yoxlayırıq
        var @event = await _eventRepository.GetAsync(e => e.Id == request.EventId && !e.IsDeleted);
        if (@event == null)
            throw new NotFoundException("Tədbir tapılmadı.");

        // Tədbiri WishList-ə əlavə edirik
        wishList.AddEventToWishList(@event);

        // Bütün dəyişiklikləri saxlamaq üçün
        await _wishListRepository.Commit(cancellationToken);

        return true;
    }
}