using Microsoft.AspNetCore.Http;

namespace MyTicket.Application.Features.Commands.Admin.Event.ViewModels;
public class EventMediaModel
{
    public string? Others { get; set; } 
    public IFormFile MainImage { get; set; }
    public List<IFormFile>? Medias { get; set; }
}

