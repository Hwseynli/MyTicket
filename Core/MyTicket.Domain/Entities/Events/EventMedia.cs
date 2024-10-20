using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Medias;

namespace MyTicket.Domain.Entities.Events;
public class EventMedia : BaseEntity
{
    public List<Media> Medias { get; private set; }
    public MediaType MediaType { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; }

    public void SetDetails(MediaType mediaType)
    {
        MediaType = mediaType;
        Medias = new List<Media>();
    }
}