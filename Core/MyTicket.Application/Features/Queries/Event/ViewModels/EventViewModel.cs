namespace MyTicket.Application.Features.Queries.Event.ViewModels;
public class EventViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string PlaceHallName { get; set; }
    public string PlaceName { get; set; }
    public int AvailableTicketCount { get; set; }
    public List<MediaViewModel> EventMedias { get; set; }


    public static EventViewModel MapToViewModel(Domain.Entities.Events.Event eventEntity)
    {
        return new EventViewModel
        {
            Id = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.IsDeleted ? "Müddəti bitib" : eventEntity.Description,
            StartTime = eventEntity.StartTime,
            EndTime = eventEntity.EndTime,
            PlaceHallName = eventEntity.PlaceHall.Name,
            PlaceName = eventEntity.PlaceHall.Place.Name,
            AvailableTicketCount = eventEntity.Tickets.Count(t => !t.IsSold && !t.IsReserved),
            EventMedias = eventEntity.EventMedias.Select(em => em.Medias.Select(m => new MediaViewModel
            {
                Name = m.Name,
                Path = m.Path,
                IsMain = m.IsMain
            }).ToList()).FirstOrDefault() ?? new List<MediaViewModel>()
        };
    }
}

