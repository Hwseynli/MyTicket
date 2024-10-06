using MyTicket.Domain.Common;

namespace MyTicket.Domain.Entities.Users;
public class Role:BaseEntity
{
    public string Name { get; set; }
    public List<User> Users { get; set; }

    public void SetDetails(string name)
    {
        Name = name;
        Users = new List<User>();
    }
}