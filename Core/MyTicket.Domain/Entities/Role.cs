namespace MyTicket.Domain.Entities;
public class Role
{
    public string Name { get; set; }

    public void SetDetails(string name)
    {
        Name = name;
    }
}