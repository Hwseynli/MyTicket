using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.PromoCode.Delete;
public class DeletePromoCodeCommandValidator : AbstractValidator<DeletePromoCodeCommand>
{
    public DeletePromoCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(UIMessage.Required("Id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Id"));
    }
}

