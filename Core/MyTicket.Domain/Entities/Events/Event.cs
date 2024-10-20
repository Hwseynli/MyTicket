using MyTicket.Domain.Entities.Categories;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Entities.Ratings;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Domain.Entities.Events;
public class Event : Editable<User>
{
    public string Title { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string Description { get; private set; }
    public List<EventMedia> EventMedias { get; private set; }
    public int SubCategoryId { get; private set; }
    public SubCategory SubCategory { get; private set; }
    public int PlaceHallId { get; private set; }
    public PlaceHall PlaceHall { get; private set; }
    public bool IsDeleted { get; private set; } // Soft deletion
    public List<Rating> Ratings { get; private set; }
    public double AverageRating { get; set; } // Ortalama reytinq

    public void SetDetails(string name, DateTime startTime, DateTime endTime, string description, int categoryId, int placeHallId, IEnumerable<Event> existingEvents)
    {
        if (startTime >= endTime)
            throw new DomainException("Başlanğıc vaxtı son vaxtdan əvvəl olmalıdır.");

        // Eyni vaxtda eyni PlaceHall-da digər event olub olmadığını yoxlayırıq
        foreach (var ev in existingEvents)
        {
            if (ev.PlaceHallId == placeHallId &&
                ((startTime >= ev.StartTime && startTime < ev.EndTime) ||
                (endTime > ev.StartTime && endTime <= ev.EndTime) ||
                (startTime <= ev.StartTime && endTime >= ev.EndTime)))
            {
                throw new DomainException("Eyni vaxtda eyni məkanda başqa bir tədbir var.");
            }
        }
        AverageRating = 0;
        Title = name;
        SubCategoryId = categoryId;
        StartTime = startTime;
        EndTime = endTime;
        Description = description;
        EventMedias = new List<EventMedia>();
        Ratings = new List<Rating>();
        PlaceHallId = placeHallId;
        IsDeleted = false;
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

