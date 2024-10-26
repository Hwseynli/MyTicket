namespace MyTicket.Application.Features.Queries.Event.ViewModels;
public class WishListEventDto
{
    public int EventId { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public double AverageRating { get; set; }
}