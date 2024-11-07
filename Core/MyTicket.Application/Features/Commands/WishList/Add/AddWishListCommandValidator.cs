using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.WishList.Add;
public class AddWishListCommandValidator : AbstractValidator<AddWishListCommand>
{
    public AddWishListCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage(UIMessage.NotEmpty("Event id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Event id"));
    }
}