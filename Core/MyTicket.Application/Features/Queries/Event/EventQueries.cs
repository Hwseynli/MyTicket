using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Event.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Queries.Event;
public class EventQueries : IEventQueries
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserManager _userManager;
    private readonly IWishListRepository _wishListRepository;

    public EventQueries(IEventRepository eventRepository, IWishListRepository wishListRepository, IUserManager userManager)
    {
        _eventRepository = eventRepository;
        _wishListRepository = wishListRepository;
        _userManager = userManager;
    }

    public async Task<double> GetRating(int eventId)
    {
        if (eventId <= 0)
            throw new BadRequestException();
        var eventEntity = await _eventRepository.GetAsync(x => x.Id == eventId);

        if (eventEntity == null)
            throw new NotFoundException("Tədbir tapılmadı.");

        // Ortalama reytinqi qaytarırıq
        return eventEntity.AverageRating;
    }

    public async Task<List<WishListEventDto>> GetWishList()
    {
        // İstifadəçi ID-sini əldə edirik
        int userId = _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException();

        // İstifadəçinin WishListini gətiririk
        var wishList = await _wishListRepository.GetAsync(x => x.UserId == userId, "WishListEvents.Event");
        if (wishList == null)
            throw new NotFoundException("İstifadəçinin WishListi tapılmadı.");

        // WishList-dəki tədbirləri DTO formatında qaytarırıq
        var wishListEventDtos = wishList.WishListEvents
            .Select(wlEvent => new WishListEventDto
            {
                EventId = wlEvent.EventId,
                Title = wlEvent.Event.Title,
                MinPrice = wlEvent.Event.MinPrice,
                StartTime = wlEvent.Event.StartTime,
                EndTime = wlEvent.Event.EndTime,
                Description = wlEvent.Event.Description,
                AverageRating = wlEvent.Event.AverageRating
            }).ToList();

        return wishListEventDtos;
    }
}