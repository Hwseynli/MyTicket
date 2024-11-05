using Microsoft.AspNetCore.Http;

namespace MyTicket.Application.Features.Commands.Admin.Event.ViewModels;
public class EventMediaUpdateModel
{
    public int Id { get; set; }
    public IFormFile? MainImage { get; set; }
    public string? Others { get; set; } = "media";
    public List<IFormFile>? Medias { get; set; }
}