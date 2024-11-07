using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Basket.AddTicket;
public class AddTicketToBasketCommandValidator : AbstractValidator<AddTicketToBasketCommand>
{
    public AddTicketToBasketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty().WithMessage(UIMessage.Required("Ticket id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Ticket id"));
    }
}

