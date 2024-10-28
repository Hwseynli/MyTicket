using FluentValidation;

namespace MyTicket.Application.Features.Commands.Basket.RemoveTicket;

public class RemoveTicketFromBasketCommandValidator : AbstractValidator<RemoveTicketFromBasketCommand>
{
    public RemoveTicketFromBasketCommandValidator()
    {
        RuleFor(x => x.TicketId).GreaterThan(0).WithMessage("Ticket Id invalid.");
    }
}

