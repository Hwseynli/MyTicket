﻿using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IRepositories.Events;

namespace MyTicket.Application.Features.Commands.Admin.Rating.GetQuery;
public class GetEventRatingQueryHandler : IRequestHandler<GetEventRatingQuery, double>
{
    private readonly IEventRepository _eventRepository;

    public GetEventRatingQueryHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<double> Handle(GetEventRatingQuery request, CancellationToken cancellationToken)
    {
        var eventEntity = await _eventRepository.GetAsync(x => x.Id == request.EventId);

        if (eventEntity == null)
            throw new NotFoundException("Tədbir tapılmadı.");

        // Ortalama reytinqi qaytarırıq
        return eventEntity.Ratings.Average(r => (int)r.RatingValue);
    }
}
