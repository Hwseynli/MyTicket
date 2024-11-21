using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Domain.Entities.Ratings;
public class Rating : BaseEntity
{
    public int UserId { get; private set; }
    public User User { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; }
    public RatingValue RatingValue { get; private set; }
    public DateTime RatedAt { get; private set; }

    public void SetRating(int ratingValue, int userId, int eventId)
    {
        if (ratingValue < 1 || ratingValue > 5)
            throw new DomainException(UIMessage.Between("Rating value",1,5));

        RatingValue = (RatingValue)ratingValue;
        RatedAt = DateTime.UtcNow.AddHours(4);
        EventId = eventId;
        UserId = userId;
    }
    public void SetRatingForUpdate(int ratingValue)
    {
        if (ratingValue < 1 || ratingValue > 5)
            throw new DomainException(UIMessage.Between("Rating value", 1, 5));

        RatingValue = (RatingValue)ratingValue;
        RatedAt = DateTime.UtcNow.AddHours(4);
    }
}