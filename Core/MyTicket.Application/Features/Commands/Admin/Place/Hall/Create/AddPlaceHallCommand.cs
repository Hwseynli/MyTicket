using MediatR;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommand : IRequest<bool>
{
    public string Name { get; set; }
    public int SeatCount { get; set; }
    public int RowCount { get; set; }
    public int PlaceId { get; set; }
}

