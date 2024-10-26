using FluentValidation;

namespace MyTicket.Application.Features.Commands.WishList.Remove;
public class RemoveFromWishListCommandValidator : AbstractValidator<RemoveFromWishListCommand>
{
    public RemoveFromWishListCommandValidator()
    {
        RuleFor(x => x.EventId)
           .GreaterThan(0).WithMessage("Tədbir ID-si 0-dan böyük olmalıdır.");
    }
}

