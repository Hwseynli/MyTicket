
using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.PromoCode.Update;
public class UpdatePromoCodeCommandValidator : AbstractValidator<UpdatePromoCodeCommand>
{
    public UpdatePromoCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(UIMessage.Required("Id"))
                .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Id"));

        RuleFor(x => x.UniqueCode)
            .Matches(@"^[a-zA-Z0-9]{5,10}$")
            .WithMessage(UIMessage.ValidProperty("Unique code"))
                .WithMessage("Unique code must be alphanumeric and 5-10 characters long.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Discount amount"));

        RuleFor(x => x.ExpirationDate)
           .GreaterThan(DateTime.Now).WithMessage(UIMessage.ValidProperty("Expiration date"));

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Usage limit"));

    }
}

