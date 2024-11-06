using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Queries.Event.ViewModels;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Queries.Event;
public class EventQueries : IEventQueries
{
    private readonly IEventRepository _eventRepository;

    public EventQueries(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<string> GetRating(int eventId)
    {
        if (eventId <= 0)
            throw new BadRequestException();
        var eventEntity = await _eventRepository.GetAsync(x => x.Id == eventId);

        if (eventEntity == null)
            throw new NotFoundException("Tədbir tapılmadı.");

        return eventEntity.GetRating(eventEntity.AverageRating);
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