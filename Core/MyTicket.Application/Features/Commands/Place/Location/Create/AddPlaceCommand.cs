using MediatR;

namespace MyTicket.Application.Features.Commands.Place.Location.Create;
public class AddPlaceCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string Address { get; set; }
}

