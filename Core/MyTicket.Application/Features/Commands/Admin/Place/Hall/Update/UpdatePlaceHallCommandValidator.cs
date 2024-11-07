using FluentValidation;
using MyTicket.Application.Features.Commands.Place.Hall.Update;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.Place.Hall.Update;
public class UpdatePlaceHallCommandValidator : AbstractValidator<UpdatePlaceHallCommand>
{
    public UpdatePlaceHallCommandValidator()
    {
        RuleFor(ph => ph.Id).GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Place hall id"));
        RuleFor(ph => ph.Name)
            .NotEmpty().WithMessage(UIMessage.Required("Place hall Name"))
            .MinimumLength(3)
            .MaximumLength(50);

        RuleFor(ph => ph.SeatCount)
            .GreaterThan(0)
            .WithMessage(UIMessage.ValidProperty("Seat count"))
            .Must((command, seatCount) => seatCount!=0 && seatCount % command.RowCount == 0)
            .WithMessage("Seat Count must be exactly divisible by Row Count.");

        RuleFor(ph => ph.RowCount)
            .GreaterThan(0)
            .WithMessage(UIMessage.ValidProperty("RowCount"));

        RuleFor(ph => ph.PlaceId)
            .GreaterThan(0)
            .WithMessage(UIMessage.ValidProperty("Place hall id"));
    }
}
