using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Places;
using MyTicket.Domain.Exceptions;

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
        int userId = _userManager.GetCurrentUserId();
        if (userId <= 0)
            throw new UnAuthorizedException();

        var placeHall = await _placeHallRepository.GetAsync(ph => ph.Id == request.Id, "Seats");
        if (placeHall == null)
            throw new NotFoundException("PlaceHall tapılmadı.");

        // Oturacaqların sayı və sıra sayı uyğun olmalıdır
        if (request.SeatCount % request.RowCount != 0)
            throw new DomainException("Oturacaqların sayı və sıra sayları uyğun deyil, tam bölünmür.");

        // Yeniləmələr tətbiq olunur
        placeHall.SetDetailsForUpdate(request.Name, request.PlaceId, request.SeatCount, request.RowCount, userId);

        // Hər bir sətir və oturacaq üçün yenilənmiş məlumatlar
        int seatsPerRow = request.SeatCount / request.RowCount;
        var seatEntities = new List<Seat>();
        for (int row = 1; row <= request.RowCount; row++)
        {
            for (int seat = 1; seat <= seatsPerRow; seat++)
            {
                var existingSeat = placeHall.Seats.FirstOrDefault(s => s.RowNumber == row && s.SeatNumber == seat);

                if (existingSeat != null)
                {
                    var seatType = existingSeat.DetermineSeatType(row, request.RowCount);
                    // Mövcud oturacağı yeniləyirik
                    existingSeat.SetDetailForUpdate(row, seat, seatType, existingSeat.CalculateSeatPrice(seatType), userId);
                }
                else
                {
                    // Yeni oturacaq əlavə edirik
                    var newSeat = new Seat();
                    var seatType = newSeat.DetermineSeatType(row, request.RowCount);
                    newSeat.SetDetail(row, seat, seatType, newSeat.CalculateSeatPrice(seatType), userId);
                    seatEntities.Add(newSeat);
                }
            }
        }

        if (seatEntities.Any())
        {
            // Yeni oturacaqları hall-a əlavə edirik
            placeHall.Seats.AddRange(seatEntities);
        }

        // Yenilənmiş Hall və Seat obyektlərini saxlayırıq

        _placeHallRepository.Update(placeHall);
        await _placeHallRepository.Commit(cancellationToken);

        await _seatRepository.Commit(cancellationToken);

        return true;
    }
}

