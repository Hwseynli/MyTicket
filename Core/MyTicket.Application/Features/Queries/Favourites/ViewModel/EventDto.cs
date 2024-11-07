namespace MyTicket.Application.Features.Queries.Favourites.ViewModel;
public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal MinPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string PlaceHallName { get; set; }
    public string PlaceName { get; set; }
    public int AvailableTicketCount { get; set; }
    public string Rating { get; set; }
    public MediaDto EventMedia { get; set; }


    public static EventDto MapToViewModel(Domain.Entities.Events.Event eventEntity)
    {
        var medias = eventEntity.EventMedias.Select(em => em.Medias.Where(x => x.IsMain).FirstOrDefault());
        return new EventDto
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            MinPrice = eventEntity.MinPrice,
            Description = eventEntity.IsDeleted ? "Expired" : eventEntity.Description,
            StartTime = eventEntity.StartTime,
            EndTime = eventEntity.EndTime,
            PlaceHallName = eventEntity.PlaceHall.Name,
            PlaceName = eventEntity.PlaceHall.Place.Name,
            AvailableTicketCount = eventEntity.Tickets.Count(t => !t.IsSold && !t.IsReserved),
            Rating = eventEntity.GetRating(eventEntity.AverageRating),
            EventMedia = medias.Select(m => new MediaDto
            {
                Name = m.Name,
                Path = m.Path,
                Others=m.Others
            }).FirstOrDefault() ?? new MediaDto()
        };
    }
}

