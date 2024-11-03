using FluentValidation;

namespace MyTicket.Application.Features.Commands.PromoCode.Update;
public class UpdatePromoCodeCommandValidator : AbstractValidator<UpdatePromoCodeCommand>
{
    public UpdatePromoCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .GreaterThan(0).WithMessage("Id amount must be greater than 1.");

        RuleFor(x => x.UniqueCode)
            .Matches(@"^[a-zA-Z0-9]{5,10}$")
            .WithMessage("Unique code must be alphanumeric and 5-10 characters long.");

        RuleFor(x => x.DiscountAmount)
            .GreaterThan(0)
            .WithMessage("Discount amount must be greater than 0.");

        RuleFor(x => x.ExpirationDate)
           .GreaterThan(DateTime.Now)
           .WithMessage("Expiration date must be in the future.");

        RuleFor(x => x.UsageLimit)
            .GreaterThan(0)
            .WithMessage("Usage limit must be greater than 0.");
    }
}

