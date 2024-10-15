using MyTicket.Domain.Entities.Users;

namespace MyTicket.Domain.Entities.Categories;
public class Category : Editable<User>
{
    public string Name { get; private set; }
    public List<SubCategory> SubCategories { get; set; }

    public void SetDetails(string name,int createdById)
    {
        Name = name;
        CreatedById = createdById;
        RecordDateTime = DateTime.UtcNow.AddHours(4);
        SubCategories = new List<SubCategory>();
    }

    public void SetDetailsForUpdate(string name, int updatedById)
    {
        Name = name;
        UpdateById = updatedById;
        LastUpdateDateTime = DateTime.UtcNow.AddHours(4);
    }

    public void AddSubCategory(SubCategory subcategory)
    {
        SubCategories.Add(subcategory);
    }
}