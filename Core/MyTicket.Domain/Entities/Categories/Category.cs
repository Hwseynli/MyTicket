using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Categories;
public class Category : Editable<User>
{
    public string Name { get; private set; }
    public List<SubCategory> SubCategories { get; set; }
    public List<Event> Events { get; set; }

    public void SetDetails(string name, int createdById)
    {
        Name = name.Capitalize();
        CreatedById = createdById;
        RecordDateTime = DateTime.UtcNow.AddHours(4);
        SubCategories = new List<SubCategory>();
        Events = new List<Event>();
        SetAuditDetails(createdById);
    }

    public void SetDetailsForUpdate(string name, int updatedById)
    {
        Name = name.Capitalize();
        SetEditFields(updatedById);
    }
}

