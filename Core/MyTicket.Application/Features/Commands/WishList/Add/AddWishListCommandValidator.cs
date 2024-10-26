using FluentValidation;

namespace MyTicket.Application.Features.Commands.WishList.Add;
public class AddWishListCommandValidator : AbstractValidator<AddWishListCommand>
{
    public AddWishListCommandValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0).WithMessage("Tədbir ID-si 0-dan böyük olmalıdır.");
    }
}