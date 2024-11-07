using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommandValidator : AbstractValidator<AddPlaceHallCommand>
{
    private readonly IPlaceHallRepository _placeHallRepository;
    public AddPlaceHallCommandValidator(IPlaceHallRepository placeHallRepository)
    {
        _placeHallRepository = placeHallRepository;

        RuleFor(ph => ph.Name).NotEmpty().MinimumLength(3).MaximumLength(100)
            .WithMessage(UIMessage.MaxLength("Place hall name", 100))
            .MustAsync(async (name, cancellationToken) => await _placeHallRepository.IsPropertyUniqueAsync(x => x.Name.ToLower(), name.Trim().ToLower()))
            .WithMessage(UIMessage.UniqueProperty("Place hall name")); ;

        RuleFor(ph => ph.SeatCount)
            .GreaterThan(0)
            .WithMessage(UIMessage.GreaterThanZero("Seat count"))
            .Must((command, seatCount) => seatCount % command.RowCount == 0)
            .WithMessage("Seat count must be divisible by row count.");

        RuleFor(ph => ph.RowCount)
            .GreaterThan(0)
            .WithMessage(UIMessage.ValidProperty("Row count"));

        RuleFor(ph => ph.PlaceId)
            .GreaterThan(0)
            .WithMessage(UIMessage.ValidProperty("Place ID"));
    }
}

