using FluentValidation;

namespace MyTicket.Application.Features.Commands.Basket.AddTicket;
public class AddTicketToBasketCommandValidator : AbstractValidator<AddTicketToBasketCommand>
{
    public AddTicketToBasketCommandValidator()
    {
        RuleFor(x => x.TicketId).GreaterThan(0).WithMessage("Ticket Id invalid.");
    }
}

