using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Categories;
public class SubCategory : Editable<User>
{
    public string Name { get; private set; }
    public int CategoryId { get; private set; }
    public Category Category { get; private set; }
    public List<Event> Events { get; set; }

    public void SetDetails(string name, int categoryId, int createdById)
    {
        Name = name;
        CategoryId = categoryId;
        Events = new List<Event>();
        SetAuditDetails(createdById);
    }
    public void SetDetailsForUpdate(string name, int categoryId, int updatedById)
    {
        Name = name;
        CategoryId = categoryId;
        SetEditFields(updatedById);
    }
}