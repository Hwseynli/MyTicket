using FluentValidation;

namespace MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
public class RateEventCommandValidator : AbstractValidator<RateEventCommand>
{
    public RateEventCommandValidator()
    {
        RuleFor(command => command.EventId).GreaterThan(0).WithMessage("UserId is required.");
        RuleFor(command => command.RatingValue).NotEmpty();
    }
}

