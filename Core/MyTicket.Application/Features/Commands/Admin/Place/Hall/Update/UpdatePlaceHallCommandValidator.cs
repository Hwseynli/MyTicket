using FluentValidation;
using MyTicket.Application.Features.Commands.Place.Hall.Update;

namespace MyTicket.Application.Features.Commands.Admin.Place.Hall.Update;
public class UpdatePlaceHallCommandValidator : AbstractValidator<UpdatePlaceHallCommand>
{
    public UpdatePlaceHallCommandValidator()
    {
        RuleFor(ph => ph.Id).GreaterThan(0).WithMessage("Hall ID valid olmalıdır.");
        RuleFor(ph => ph.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(ph => ph.SeatCount)
            .GreaterThan(0)
            .WithMessage("SeatCount valid olmalıdır.")
            .Must((command, seatCount) => seatCount!=0 && seatCount % command.RowCount == 0)
            .WithMessage("SeatCount RowCount-a tam bölünməlidir.");

        RuleFor(ph => ph.RowCount)
            .GreaterThan(0)
            .WithMessage("RowCount valid olmalıdır.");

        RuleFor(ph => ph.PlaceId)
            .GreaterThan(0)
            .WithMessage("PlaceId valid olmalıdır.");
    }
}
