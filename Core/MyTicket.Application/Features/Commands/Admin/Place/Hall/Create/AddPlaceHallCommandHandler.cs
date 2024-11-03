using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommandHandler : IRequestHandler<AddPlaceHallCommand, bool>
{
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IUserManager _userManager;

    public AddPlaceHallCommandHandler(IPlaceHallRepository placeHallRepository, IUserManager userManager, ISeatRepository seatRepository)
    {
        _placeHallRepository = placeHallRepository;
        _userManager = userManager;
        _seatRepository = seatRepository;
    }

    public async Task<bool> Handle(AddPlaceHallCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();

        if (request.SeatCount % request.RowCount != 0)
            throw new DomainException("Oturacaqların sayı və sıra sayları uyğun deyil, tam bölünmür.");

        var placeHall = new PlaceHall();
        placeHall.SetDetails(request.Name, request.PlaceId, request.SeatCount, request.RowCount, userId);
        await _placeHallRepository.AddAsync(placeHall);
        await _placeHallRepository.Commit(cancellationToken);

        //yeni oturacaqları yaradaq
        await _seatRepository.CreatSeatsAsync(request.SeatCount, request.RowCount, placeHall.Id, userId, cancellationToken);
        return true;
    }
}
