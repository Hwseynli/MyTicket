﻿using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Queries.Basket;
using MyTicket.Application.Features.Queries.Event;

namespace MyTicket.Controllers;
[Route("api/events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventQueries _eventQueries;
    private readonly ITicketQueries _ticketQueries;

    public EventsController(IEventQueries eventQueries, ITicketQueries ticketQueries)
    {
        _eventQueries = eventQueries;
        _ticketQueries = ticketQueries;
    }

    // GET: api/values
    [HttpGet("get-rating-by-{eventId}")]
    public async Task<IActionResult> GetRating(int eventId)
    {
        var result = await _eventQueries.GetRating(eventId);
        return Ok(result);
    }

    // Bütün tədbirləri gətir
    [HttpGet]
    public async Task<IActionResult> GetAllEvents()
    {
        var events = await _eventQueries.GetAllEventsAsync();
        return Ok(events);
    }

    // Tədbiri ID-yə görə gətir
    [HttpGet("get-event-by-{eventId}")]
    public async Task<IActionResult> GetEventById(int eventId)
    {
        var eventItem = await _eventQueries.GetEventByIdAsync(eventId);
        if (eventItem == null)
        {
            return NotFound("Tədbir tapılmadı.");
        }
        return Ok(eventItem);
    }

    // Başlıq üzrə axtarış
    [HttpGet("search-by-title")]
    public async Task<IActionResult> GetEventsByTitle(string title)
    {
        var events = await _eventQueries.GetEventsByTitleAsync(title);
        return Ok(events);
    }

    // Məkan üzrə axtarış
    [HttpGet("search-by-place")]
    public async Task<IActionResult> GetEventsByPlace(int placeHallId)
    {
        var events = await _eventQueries.GetEventsByPlaceAsync(placeHallId);
        return Ok(events);
    }

    // Qiymət aralığı üzrə axtarış
    [HttpGet("search-by-price")]
    public async Task<IActionResult> GetEventsByPriceRange(decimal? minPrice, decimal? maxPrice)
    {
        var events = await _eventQueries.GetEventsByPriceRangeAsync(minPrice, maxPrice);
        return Ok(events);
    }

    [HttpGet("get-sold-tickets-for-{eventId}")]
    public async Task<IActionResult> GetSoldTicketsForEvent(int eventId)
    {
        var result = await _ticketQueries.GetSoldTicketsForEvent(eventId);
        return Ok(result);
    }

    [HttpGet("get-all-tickets-for-{eventId}")]
    public async Task<IActionResult> GetAllTicketsForEvent(int eventId)
    {
        var result = await _ticketQueries.GetSoldTicketsForEvent(eventId);
        return Ok(result);
    }
}