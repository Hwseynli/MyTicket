using MediatR;
using MyTicket.Application.Features.Commands.Admin.Event.ViewModels;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.Admin.Event.Update;
public class UpdateEventCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal MinPrice { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public LanguageType Language { get; set; }
    public byte MinAge { get; set; }
    public int PlaceHallId { get; set; }
    public int CategoryId { get; set; }
    public List<int> SubCategoryIds { get; set; }
    public List<EventMediaUpdateModel> EventMediaModels { get; set; }
    public List<int>? DeletedMediaIds { get; set; }
    public bool IsDeleted { get; set; }
}

