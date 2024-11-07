using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
public class RateEventCommandValidator : AbstractValidator<RateEventCommand>
{
    public RateEventCommandValidator()
    {
        RuleFor(command => command.EventId).GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Event id"));
        RuleFor(command => command.RatingValue).NotEmpty().WithMessage(UIMessage.Required("Rating value"));
    }
}

