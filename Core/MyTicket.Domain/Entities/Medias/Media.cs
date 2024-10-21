using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Medias;
public class Media : Editable<User>
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public bool IsMain { get; private set; }
    public int EventMediaId { get; private set; }
    public EventMedia EventMedia { get; private set; }

    public void SetDetails(string name, string url, bool isMain=false)
    {
        Name = name;
        Path = url;
        IsMain = isMain;
    }
}

