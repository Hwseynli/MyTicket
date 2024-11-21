using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Exceptions;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Place.Hall.Update;
public class UpdatePlaceHallCommandHandler : IRequestHandler<UpdatePlaceHallCommand, bool>
{
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IUserManager _userManager;

    public UpdatePlaceHallCommandHandler(IPlaceHallRepository placeHallRepository, IUserManager userManager, ISeatRepository seatRepository)
    {
        _placeHallRepository = placeHallRepository;
        _userManager = userManager;
        _seatRepository = seatRepository;
    }

    public async Task<bool> Handle(UpdatePlaceHallCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();

        var placeHall = await _placeHallRepository.GetAsync(ph => ph.Id == request.Id, "Seats");
        if (placeHall == null)
            throw new NotFoundException(UIMessage.NotFound("Place Hall"));

        if (!await _placeHallRepository.IsPropertyUniqueAsync(x => x.Name, request.Name, placeHall.Id))
            throw new BadRequestException(UIMessage.AlreadyExist($"{request.Name}"));

        // Oturacaqların sayı və sıra sayı uyğun olmalıdır
        if (request.SeatCount % request.RowCount != 0)
            throw new DomainException("The number of seats and the number of rows do not match, they are not fully divided.");

        // Yeniləmələr tətbiq olunur
        placeHall.SetDetailsForUpdate(request.Name, request.PlaceId, request.SeatCount, request.RowCount, userId);

        // Yenilənmiş Hall obyektini saxlayırıq
        await _placeHallRepository.Update(placeHall);
        await _placeHallRepository.Commit(cancellationToken);

        if (request.SeatCount != placeHall.SeatCount || request.RowCount != placeHall.RowCount)
        {
            await _seatRepository.RemoveRange(placeHall.Seats);
            await _seatRepository.Commit(cancellationToken);
            // Hər bir sətir və oturacaq üçün yenilənmiş məlumatlar
            await _seatRepository.CreatSeatsAsync(request.SeatCount, request.RowCount, placeHall.Id, userId, cancellationToken);
        }
        return true;
    }
}