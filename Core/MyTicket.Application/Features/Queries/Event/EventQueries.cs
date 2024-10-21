using System;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Queries.Event;
public class EventQueries : IEventQueries
{
    private readonly IEventRepository _eventRepository;

    public EventQueries(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
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
}

