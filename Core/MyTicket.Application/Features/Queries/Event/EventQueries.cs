using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Event.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Enums;

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

    public async Task<string> GetRating(int eventId)
    {
        if (eventId <= 0)
            throw new BadRequestException();
        var eventEntity = await _eventRepository.GetAsync(x => x.Id == eventId);

        if (eventEntity == null)
            throw new NotFoundException("Tədbir tapılmadı.");

        switch (eventEntity.AverageRating)
        {
            case 0:
                return $"{RatingValue.NotRated}";
            case 1:
                return $"{RatingValue.OneStar}";
            case 2:
                return $"{RatingValue.TwoStars}";
            case 3:
                return $"{RatingValue.ThreeStars}";
            case 4:
                return $"{RatingValue.FourStars}";
            case 5:
                return $"{RatingValue.FiveStars}";
            default:
                throw new NotFoundException();
        }
    }

    public async Task<List<WishListEventDto>> GetWishList()
    {
        // İstifadəçi ID-sini əldə edirik
        int userId = await _userManager.GetCurrentUserId();
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

    public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync()
    {
        var events = await _eventRepository.GetAllAsync(null, "EventMedias", "EventMedias.Medias", "PlaceHall", "PlaceHall.Place", "Tickets");
        return events.Select(e => EventViewModel.MapToViewModel(e));
    }

    public async Task<EventViewModel?> GetEventByIdAsync(int eventId)
    {
        var eventEntity = await _eventRepository.GetAsync(e => e.Id == eventId, "EventMedias", "EventMedias.Medias", "PlaceHall", "PlaceHall.Place", "Tickets");
        return eventEntity != null ? EventViewModel.MapToViewModel(eventEntity) : null;
    }

    public async Task<IEnumerable<EventViewModel>> GetEventsByTitleAsync(string title)
    {
        var events = await _eventRepository.GetAllAsync(null, "EventMedias", "EventMedias.Medias", "PlaceHall", "PlaceHall.Place", "Tickets");

        // Bütün Event-ləri yaddaşa gətirdikdən sonra filtre tətbiq olunur
        var filteredEvents = events
            .Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase))
            .Select(e => EventViewModel.MapToViewModel(e));

        return filteredEvents;
    }

    public async Task<IEnumerable<EventViewModel>> GetEventsByPlaceAsync(int placeHallId)
    {
        var events = await _eventRepository.GetAllAsync(e => e.PlaceHallId == placeHallId, "EventMedias", "EventMedias.Medias", "PlaceHall", "PlaceHall.Place", "Tickets");
        return events.Select(e => EventViewModel.MapToViewModel(e));
    }

    public async Task<IEnumerable<EventViewModel>> GetEventsByPriceRangeAsync(decimal? minPrice, decimal? maxPrice)
    {
        if (!minPrice.HasValue && !maxPrice.HasValue)
        {
            return await GetAllEventsAsync();
        }
        else if (minPrice.HasValue && maxPrice.HasValue && maxPrice > minPrice)
        {
            var events = await _eventRepository.GetAllAsync(e => e.MinPrice >= minPrice && e.MinPrice <= maxPrice, "EventMedias", "EventMedias.Medias", "PlaceHall", "PlaceHall.Place", "Tickets");
            return events.Select(e => EventViewModel.MapToViewModel(e));
        }
        else
        {
            var events = await _eventRepository.GetAllAsync(e => (minPrice.HasValue && e.MinPrice >= minPrice) ||
                                                                 (maxPrice.HasValue && e.MinPrice <= maxPrice), "EventMedias", "EventMedias.Medias", "PlaceHall", "PlaceHall.Place", "Tickets");

            return events.Select(e => EventViewModel.MapToViewModel(e));
        }
    }
}