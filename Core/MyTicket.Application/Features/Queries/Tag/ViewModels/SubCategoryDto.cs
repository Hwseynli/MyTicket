namespace MyTicket.Application.Features.Queries.Tag.ViewModels;
public class SubCategoryDto
{
    public string Name { get; set; }
    public List<string> CategoryNames { get; set; }
    public int EventsCount { get; set; }
}