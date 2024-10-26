using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Commands.WishList.Remove;
public class RemoveFromWishListCommandHandler : IRequestHandler<RemoveFromWishListCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IWishListRepository _wishListRepository;
    private readonly IUserManager _userManager;

    public RemoveFromWishListCommandHandler(IEventRepository eventRepository, IWishListRepository wishListRepository, IUserManager userManager)
    {
        _eventRepository = eventRepository;
        _wishListRepository = wishListRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(RemoveFromWishListCommand request, CancellationToken cancellationToken)
    {
        int userId = _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException();
        // İstifadəçinin mövcud WishListini tapırıq
        var wishList = await _wishListRepository.GetAsync(x => x.UserId == userId, "WishListEvents");

        if (wishList == null)
            throw new NotFoundException("İstifadəçinin WishListi tapılmadı.");

        // Tədbirin mövcudluğunu yoxlayırıq
        var @event = await _eventRepository.GetAsync(e => e.Id == request.EventId && !e.IsDeleted);
        if (@event == null)
            throw new NotFoundException("Tədbir tapılmadı.");

        // Tədbiri WishList-dən çıxarırıq
        wishList.RemoveEventFromWishList(request.EventId);

        // Dəyişiklikləri yadda saxlamaq üçün
        await _wishListRepository.Commit(cancellationToken);

        return true;
    }
}

