using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Categories;
public class SubCategory : Editable<User>
{
    public string Name { get; private set; }
    public List<Category> Categories { get; set; }
    public List<Event> Events { get; set; }

    public void SetDetails(string name, IEnumerable<Category> categories, int createdById)
    {
        Name = name.Capitalize();
        Categories = categories.ToList();
        Events = new List<Event>();
        SetAuditDetails(createdById);
    }
    public void SetDetailsForUpdate(string name, IEnumerable<Category> categories, int updatedById)
    {
        Name = name.Capitalize();
        Categories = categories.ToList();
        SetEditFields(updatedById);
    }
}

