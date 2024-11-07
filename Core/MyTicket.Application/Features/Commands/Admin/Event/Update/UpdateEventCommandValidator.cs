using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.Event.Update;
public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(UIMessage.NotEmpty("Event Title"))
            .MaximumLength(100).WithMessage(UIMessage.MaxLength("Event title", 100));

        RuleFor(x => x.MinPrice)
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Price"));

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime).WithMessage("Event start time cannot be in the past.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage(UIMessage.GreaterThan("Event end time", "Event start time"));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(UIMessage.MaxLength("Event description", 500));

        RuleFor(x => x.MinAge)
            .GreaterThanOrEqualTo((byte)0).WithMessage(UIMessage.GreaterThan("Min age","0"));

        RuleFor(x => x.PlaceHallId)
            .GreaterThan(0).WithMessage(UIMessage.ValidProperty("Place Hall ID"));
        RuleFor(x => x.SubCategoryId)
            .GreaterThan(0).WithMessage(UIMessage.ValidProperty("Subcategory ID "));
    }
}

