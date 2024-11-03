using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommandValidator : AbstractValidator<AddPlaceHallCommand>
{
    private readonly IPlaceHallRepository _placeHallRepository;
    public AddPlaceHallCommandValidator(IPlaceHallRepository placeHallRepository)
    {
        _placeHallRepository = placeHallRepository;

        RuleFor(ph => ph.Name).NotEmpty().MinimumLength(3).MaximumLength(100)
            .WithMessage("min 3 max 100 simvoldan ibarət olmalıdır.")
            .MustAsync(async (name, cancellationToken) => await _placeHallRepository.IsPropertyUniqueAsync(x => x.Name.ToLower(), name.Trim().ToLower()))
            .WithMessage("Bu başlıqda placeHall yaradılıb istəyirsinizsə update edin ya da başqa ad ilə yaradın"); ;

        RuleFor(ph => ph.SeatCount)
            .GreaterThan(0)
            .WithMessage("SeatCount valid olmalıdır.")
            .Must((command, seatCount) => seatCount % command.RowCount == 0)
            .WithMessage("SeatCount RowCount-a tam bölünməlidir.");

        RuleFor(ph => ph.RowCount)
            .GreaterThan(0)
            .WithMessage("RowCount valid olmalıdır.");

        RuleFor(ph => ph.PlaceId)
            .GreaterThan(0)
            .WithMessage("PlaceId valid olmalıdır.");
    }
}

