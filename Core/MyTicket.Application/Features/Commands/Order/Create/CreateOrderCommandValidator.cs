using FluentValidation;

namespace MyTicket.Application.Features.Commands.Order.Create;
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Doğru e-poçt ünvanı daxil edin.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
