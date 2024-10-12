using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Seat.Update;
public class UpdateSeatCommandHandler : IRequestHandler<UpdateSeatCommand, bool>
{
    private readonly ISeatRepository _seatRepository;
    private readonly IUserManager _userManager;

    public UpdateSeatCommandHandler(ISeatRepository seatRepository, IUserManager userManager)
    {
        _seatRepository = seatRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateSeatCommand request, CancellationToken cancellationToken)
    {
        var seat = await _seatRepository.GetAsync(s => s.Id == request.Id);

        if (seat == null)
            throw new NotFoundException("Seat not found.");

        seat.SetDetail(request.Id,request.SeatNumber,request.SeatType,request.Price,request.PlaceHallId);
        seat.SetEditFields(_userManager.GetCurrentUserId());

        await _seatRepository.Update(seat);
        await _seatRepository.Commit(cancellationToken);

        return true;
    }
}
