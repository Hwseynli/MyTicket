using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Domain.Entities.Ratings;
public class Rating : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public RatingValue RatingValue { get; set; } // 1-5 ulduzlu qiymət
    public DateTime RatedAt { get; set; } // Qiymətləndirmə tarixi

    public void SetRating(int ratingValue)
    {
        if (ratingValue < 1 || ratingValue > 5)
            throw new DomainException("Reytinq 1-dən 5-ə qədər olmalıdır.");

        RatingValue = (RatingValue)ratingValue;
        RatedAt = DateTime.UtcNow;
    }
}

