using MediatR;
using Microsoft.AspNetCore.Http;
using MyTicket.Application.Features.Commands.Event.ViewModels;

namespace MyTicket.Application.Features.Commands.Event.Update;
public class UpdateEventCommand : IRequest<bool>
{
    public int Id { get; set; }  // Event ID
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public IFormFile? MainImage { get; set; } // Əsas şəkil
    public List<EventMediaModel>? EventMedias { get; set; } = new List<EventMediaModel>(); // Digər media faylları
    public List<int>? DeletedMediaIds { get; set; } = new List<int>(); // Silinən media fayllarının ID-ləri
    public int PlaceHallId { get; set; }

}

