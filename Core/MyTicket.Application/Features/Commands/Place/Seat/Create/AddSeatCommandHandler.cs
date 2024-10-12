using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Seat.Create;
public class AddSeatCommandHandler : IRequestHandler<AddSeatCommand, bool>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IUserManager _userManager;

    public AddSeatCommandHandler(ISeatRepository seatRepository, IUserManager userManager)
    {
        _seatRepository = seatRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(AddSeatCommand request, CancellationToken cancellationToken)
    {
        var seat = new Domain.Entities.Places.Seat();
        seat.SetDetail(request.RowNumber, request.SeatNumber, request.SeatType, request.Price, request.PlaceHallId);
        seat.SetAuditDetails(_userManager.GetCurrentUserId());
        await _seatRepository.AddAsync(seat);
        await _seatRepository.Commit(cancellationToken);
        return true;
    }
}

