using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Medias;
public class Media : Editable<User>
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public string? Others { get; private set; }
    public bool IsMain { get; private set; }
    public MediaType MediaType { get; private set; }
    public int EventMediaId { get; private set; }
    public EventMedia EventMedia { get; private set; }

    public void SetDetails(MediaType mediaType, string name, string url, string? others, int userId, bool isMain=false)
    {
        MediaType = mediaType;
        Name = name.Capitalize();
        Path = url;
        Others = others;
        IsMain = isMain;
        SetAuditDetails(userId);
    }
}

