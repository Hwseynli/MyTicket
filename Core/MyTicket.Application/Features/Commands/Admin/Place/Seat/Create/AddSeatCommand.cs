using MediatR;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Commands.Place.Seat.Create;
public class AddSeatCommand : IRequest<bool>
{
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public SeatType SeatType { get; set; }
    public decimal Price { get; set; }
    public int PlaceHallId { get; set; }
}

