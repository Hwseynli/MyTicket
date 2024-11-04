﻿using MyTicket.Application.Features.Queries.Event.ViewModels;

namespace MyTicket.Application.Features.Queries.Event;
public interface IEventQueries
{
    Task<double> GetRating(int eventId);
    Task<List<WishListEventDto>> GetWishList();

    Task<IEnumerable<EventViewModel>> GetAllEventsAsync();
    Task<EventViewModel?> GetEventByIdAsync(int eventId);
    Task<IEnumerable<EventViewModel>> GetEventsByTitleAsync(string title);
    Task<IEnumerable<EventViewModel>> GetEventsByPlaceAsync(int placeHallId);
    Task<IEnumerable<EventViewModel>> GetEventsByPriceRangeAsync(decimal? minPrice, decimal? maxPrice);
}

