using MediatR;
using MyTicket.Application.Features.Commands.Admin.Event.ViewModels;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.Admin.Event.Create;
public class CreateEventCommand:IRequest<bool>
{
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public LanguageType Language { get; set; }
    public byte MinAge { get; set; }
    public int PlaceHallId { get; set; }
    public int SubCategoryId { get; set; }

    public List<EventMediaModel> EventMediaModels { get; set; }
}

