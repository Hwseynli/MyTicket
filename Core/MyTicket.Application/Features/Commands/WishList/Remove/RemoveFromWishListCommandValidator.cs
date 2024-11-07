using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.WishList.Remove;
public class RemoveFromWishListCommandValidator : AbstractValidator<RemoveFromWishListCommand>
{
    public RemoveFromWishListCommandValidator()
    {
        RuleFor(x => x.EventId).NotEmpty().WithMessage(UIMessage.NotEmpty("Event id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Event id"));
    }
}

