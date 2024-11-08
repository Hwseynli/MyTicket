using MyTicket.Domain.Entities.Categories;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Favourites;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Entities.Ratings;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Events;
public class Event : Editable<User>
{
    public string Title { get; private set; }
    public decimal MinPrice { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Description { get; private set; }
    public LanguageType Language { get; set; }
    public byte MinAge { get; set; }
    public List<EventMedia> EventMedias { get; private set; }
    public List<SubCategory> SubCategories { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public int PlaceHallId { get; private set; }
    public PlaceHall PlaceHall { get; private set; }
    public bool IsDeleted { get; private set; } 
    public List<WishListEvent> WishListEvents { get; private set; }
    public List<Rating> Ratings { get; private set; }
    public List<Ticket> Tickets { get; private set; }
    public double AverageRating { get; set; }

    public void SetDetails(string name, decimal minPrice, DateTime startTime, DateTime endTime, string description, int categoryId, IEnumerable<SubCategory> subCategories, int placeHallId, double averageRating, LanguageType language, byte minAge, int userId)
    {
        if (startTime >= endTime)
            throw new DomainException("The start time must be before the end time.");

        Language = language;
        MinAge = minAge;
        AverageRating = averageRating;
        Title = name.Capitalize();
        CategoryId = categoryId;
        StartTime = startTime;
        EndTime = endTime;
        Description = description;
        EventMedias = new List<EventMedia>();
        Ratings = new List<Rating>();
        Tickets = new List<Ticket>();
        WishListEvents = new List<WishListEvent>();
        SubCategories = subCategories.ToList();
        PlaceHallId = placeHallId;
        IsDeleted = false;
        MinPrice = minPrice;
        SetAuditDetails(userId);
    }

    public void SetRatingsInEvent(Rating rating)
    {
        Ratings.Add(rating);
        AverageRating = (AverageRating * (Ratings.Count - 1) + (int)rating.RatingValue) / Ratings.Count;
    }

    public void SetDetailsForUpdate(string name, decimal minPrice, DateTime startTime, DateTime endTime, string description, List<EventMedia> eventMedias, int categoryId, IEnumerable<SubCategory> subCategories, int placeHallId, double averageRating, LanguageType language, byte minAge, int updatedById)
    {
        if (string.IsNullOrEmpty(name) || !eventMedias.Any(x => x.Medias.Any(c => c.IsMain)))
            throw new DomainException(UIMessage.NotEmpty("Event name and main image"));
        if (startTime >= endTime)
            throw new DomainException("The start time must be before the end time.");

        Title = name.Capitalize();
        MinPrice = minPrice;
        StartTime = startTime;
        EndTime = endTime;
        Description = description;
        EventMedias = eventMedias ?? new List<EventMedia>();
        CategoryId = categoryId;
        SubCategories = subCategories.ToList();
        PlaceHallId = placeHallId;
        AverageRating = averageRating;
        Language = language;
        MinAge = minAge;
        SetEditFields(updatedById);
    }

    public void SoftDelete(int userId)
    {
        IsDeleted = true;
        SetEditFields(userId);
    }

    public string GetRating(double averageRating)
    {
        switch (averageRating)
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
                throw new DomainException(UIMessage.NotFound($"averageRating={averageRating}"));
        }
    }
}

