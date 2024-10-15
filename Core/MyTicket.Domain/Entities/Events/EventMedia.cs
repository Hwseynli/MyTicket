using System.ComponentModel.DataAnnotations.Schema;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Events;
public class EventMedia : Editable<User>
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public bool IsMain { get; set; }
    public MediaType MediaType { get; private set; }
    public int EventId { get; private set; }
    public Event Event { get; private set; }

    public void SetDetails(string name, string path, MediaType mediaType, bool isMain=false)
    {
        Name = name;
        Path = path;
        MediaType = mediaType;
        IsMain = isMain;
    }
}

