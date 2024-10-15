using MediatR;
using Microsoft.AspNetCore.Http;
using MyTicket.Application.Features.Commands.Event.ViewModels;

namespace MyTicket.Application.Features.Commands.Event.Create;
public class CreateEventCommand : IRequest<bool>
{
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public IFormFile MainImage { get; set; } // Əsas şəkil
    public List<EventMediaModel> EventMedias { get; set; } // Digər media faylları
    public int PlaceHallId { get; set; }
}