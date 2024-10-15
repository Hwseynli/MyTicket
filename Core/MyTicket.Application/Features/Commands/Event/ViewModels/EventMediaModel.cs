using Microsoft.AspNetCore.Http;

namespace MyTicket.Application.Features.Commands.Event.ViewModels;
public class EventMediaModel
{
    public int MediaTypeId { get; set; } // Media növü (şəkil və ya video)
    public IFormFile File { get; set; } // Fayl
}

