using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Basket.RemoveTicket;

public class RemoveTicketFromBasketCommandValidator : AbstractValidator<RemoveTicketFromBasketCommand>
{
    public RemoveTicketFromBasketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty().WithMessage(UIMessage.Required("Ticket id"))
                   .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Ticket id"));
    }
}

