using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Application.Features.Commands.Rating.RateEvent;
public class RateEventCommandHandler : IRequestHandler<RateEventCommand, bool>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUserManager _userManager;

    public RateEventCommandHandler(IEventRepository eventRepository, IUserManager userManager)
    {
        _eventRepository = eventRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(RateEventCommand request, CancellationToken cancellationToken)
    {
        var userId = _userManager.GetCurrentUserId();
        var eventEntity = await _eventRepository.GetAsync(x => x.Id == request.EventId);

        if (eventEntity == null)
            throw new DomainException("Tədbir tapılmadı.");

        // Tədbir üçün yeni reytinq əlavə edirik
        var rating = new Domain.Entities.Events.Rating
        {
            UserId = userId,
            EventId = request.EventId,
            RatingValue = (RatingValue)request.RatingValue,
            RatedAt = DateTime.UtcNow
        };

        eventEntity.Ratings.Add(rating);

        // Ortalama reytinqi yenidən hesablayırıq
        var averageRating = eventEntity.Ratings.Average(r => (int)r.RatingValue);
        eventEntity.AverageRating = averageRating;

        await _eventRepository.Update(eventEntity);
        await _eventRepository.Commit(cancellationToken);

        return true;
    }
}

