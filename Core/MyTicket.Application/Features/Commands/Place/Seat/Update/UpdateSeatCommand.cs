using MediatR;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.Place.Seat.Update;
public class UpdateSeatCommand : IRequest<bool>
{
    public int Id { get; set; }
    public int RowNumber { get; private set; }
    public int SeatNumber { get; private set; }
    public SeatType SeatType { get; set; }
    public decimal Price { get; private set; }
    public int PlaceHallId { get; private set; }
}

