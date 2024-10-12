using FluentValidation;

namespace MyTicket.Application.Features.Commands.Place.Seat.Create;
public class AddSeatCommandValidator : AbstractValidator<AddSeatCommand>
{
    public AddSeatCommandValidator()
    {
        RuleFor(s => s.RowNumber).GreaterThan(0).WithMessage("Row number should be greater than 0.");
        RuleFor(s => s.SeatNumber).GreaterThan(0).WithMessage("Seat number should be greater than 0.");
        RuleFor(s => s.Price).GreaterThan(0).WithMessage("Price should be greater than 0.");
        RuleFor(s => s.SeatType).IsInEnum().WithMessage("Invalid seat type.");
        RuleFor(s => s.PlaceHallId).GreaterThan(0).WithMessage("PlaceHallId should be valid.");
    }
}

