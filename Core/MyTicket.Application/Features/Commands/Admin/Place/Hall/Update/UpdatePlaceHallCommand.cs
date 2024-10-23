using MediatR;

namespace MyTicket.Application.Features.Commands.Place.Hall.Update;
public class UpdatePlaceHallCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SeatCount { get; set; }
    public int RowCount { get; set; }
    public int PlaceId { get; set; }
}

