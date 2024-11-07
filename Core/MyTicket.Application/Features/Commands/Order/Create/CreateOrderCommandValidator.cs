using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Order.Create;
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        //RuleFor(x => x.Token)
        //    .NotEmpty()
        //    .WithMessage("Token must be correct");
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage(UIMessage.ValidProperty("Email"))
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
