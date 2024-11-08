
namespace MyTicket.Application.Features.Queries.Tag.ViewModels;
public class CategoryDto
{
    public string Name { get; set; }
    public List<string> SubCategoryNames { get; set; }
    public int EventsCount { get; set; }
}

