using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
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
        var userId = await _userManager.GetCurrentUserId();
        var eventEntity = await _eventRepository.GetAsync(x => x.Id == request.EventId,"Ratings");

        if (eventEntity == null)
            throw new DomainException("Tədbir tapılmadı.");

        // Check if the user has already rated the event
        var existingRating = eventEntity.Ratings.FirstOrDefault(r => r.UserId == userId);

        if (existingRating != null)
        {
            // Update the existing rating
            existingRating.SetRatingForUpdate((int)request.RatingValue);
        }
        else
        {
            // Add a new rating if no existing rating is found
            var newRating = new Domain.Entities.Ratings.Rating();
            newRating.SetRating((int)request.RatingValue, userId, request.EventId);
            eventEntity.SetRatingsInEvent(newRating);
        }

        // Recalculate average rating
        eventEntity.AverageRating = eventEntity.Ratings.Average(r => (int)r.RatingValue);

        await _eventRepository.Update(eventEntity);
        await _eventRepository.Commit(cancellationToken);

        return true;
    }
}