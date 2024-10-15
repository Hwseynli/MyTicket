using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Categories;
public class SubCategory : Editable<User>
{
    public string Name { get; private set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public void SetDetails(string name, int categoryId,int createdById)
    {
        Name = name;
        CategoryId = categoryId;
        SetAuditDetails(createdById);
    }
    public void SetDetailsForUpdate(string name, int categoryId,int updatedById)
    {
        Name = name;
        CategoryId = categoryId;
        SetEditFields(updatedById);
    }
}