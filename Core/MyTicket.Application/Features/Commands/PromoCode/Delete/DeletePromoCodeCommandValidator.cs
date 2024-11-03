using FluentValidation;

namespace MyTicket.Application.Features.Commands.PromoCode.Delete;
public class DeletePromoCodeCommandValidator : AbstractValidator<DeletePromoCodeCommand>
{
    public DeletePromoCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .GreaterThan(0).WithMessage("Id amount must be greater than 1.");
    }
}

