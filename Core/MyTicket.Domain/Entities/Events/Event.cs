using MyTicket.Domain.Entities.Categories;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Favourites;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Entities.Ratings;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

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
    public int SubCategoryId { get; private set; }
    public SubCategory SubCategory { get; private set; }
    public int PlaceHallId { get; private set; }
    public PlaceHall PlaceHall { get; private set; }
    public bool IsDeleted { get; private set; } // Soft deletion
    public List<WishListEvent> WishListEvents { get; private set; }
    public List<Rating> Ratings { get; private set; }
    public List<Ticket> Tickets { get; private set; }
    public double AverageRating { get; set; } // Ortalama reytinq

    public void SetDetails(string name, decimal minPrice, DateTime startTime, DateTime endTime, string description, int categoryId, int placeHallId, double averageRating, LanguageType language, byte minAge, int userId)
    {
        if (startTime >= endTime)
            throw new DomainException("Başlanğıc vaxtı son vaxtdan əvvəl olmalıdır.");

        Language = language;
        MinAge = minAge;
        AverageRating = averageRating;
        Title = name;
        SubCategoryId = categoryId;
        StartTime = startTime;
        EndTime = endTime;
        Description = description;
        EventMedias = new List<EventMedia>();
        Ratings = new List<Rating>();
        Tickets = new List<Ticket>();
        WishListEvents = new List<WishListEvent>();
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

    public void SetDetailsForUpdate(string name, DateTime startTime, DateTime endTime, string description, List<EventMedia> eventMedias, int categoryId, int updatedById)
    {
        if (string.IsNullOrEmpty(name) || !eventMedias.Any(x => x.Medias.Any(c => c.IsMain)))
            throw new DomainException("Tədbirin adı və əsas şəkli boş ola bilməz.");
        if (startTime >= endTime)
            throw new DomainException("Başlanğıc vaxtı son vaxtdan əvvəl olmalıdır.");

        Title = name;
        StartTime = startTime;
        EndTime = endTime;
        Description = description;
        EventMedias = eventMedias ?? new List<EventMedia>();
        SubCategoryId = categoryId;
        SetEditFields(updatedById);
    }

    // Tədbirin soft deletion (silinməsi)
    public void SoftDelete(int userId)
    {
        IsDeleted = true;
        SetEditFields(userId);
    }
}

