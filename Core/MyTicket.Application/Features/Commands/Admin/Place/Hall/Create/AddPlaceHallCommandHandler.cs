using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommandHandler : IRequestHandler<AddPlaceHallCommand, bool>
{
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly IUserManager _userManager;

    public AddPlaceHallCommandHandler(IPlaceHallRepository placeHallRepository, IUserManager userManager)
    {
        _placeHallRepository = placeHallRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(AddPlaceHallCommand request, CancellationToken cancellationToken)
    {
        int userId = _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException();

        if (request.SeatCount % request.RowCount != 0)
            throw new DomainException("Oturacaqların sayı və sıra sayları uyğun deyil, tam bölünmür.");

        var placeHall = new PlaceHall();
        placeHall.SetDetails(request.Name, request.PlaceId, request.SeatCount, request.RowCount, userId);

        int seatsPerRow = request.SeatCount/ request.RowCount;
        var seatEntities = new List<Seat>();
        for (int row = 1; row <= request.RowCount; row++)
        {
            for (int seat = 1; seat <= seatsPerRow; seat++)
            {
                var seatEntity = new Seat();
                SeatType seatType = seatEntity.DetermineSeatType(row, request.RowCount);
                seatEntity.SetDetail(row, seat, seatType , seatEntity.CalculateSeatPrice(seatType), userId);
                seatEntities.Add(seatEntity);
            }
        }
        placeHall.Seats.AddRange(seatEntities);
        await _placeHallRepository.AddAsync(placeHall);
        await _placeHallRepository.Commit(cancellationToken);

        return true;
    }
}
