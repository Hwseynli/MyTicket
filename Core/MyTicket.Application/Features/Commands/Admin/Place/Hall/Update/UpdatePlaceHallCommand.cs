using MediatR;

namespace MyTicket.Application.Features.Commands.Place.Hall.Update;
public class UpdatePlaceHallCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string HallName { get; set; }
    public int PlaceId { get; set; }
}

