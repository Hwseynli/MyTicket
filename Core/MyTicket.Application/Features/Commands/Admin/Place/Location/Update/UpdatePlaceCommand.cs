using MediatR;

namespace MyTicket.Application.Features.Commands.Place.Location.Update;
public class UpdatePlaceCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
}

