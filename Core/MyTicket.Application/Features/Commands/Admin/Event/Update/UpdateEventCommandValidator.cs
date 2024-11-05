using FluentValidation;

namespace MyTicket.Application.Features.Commands.Admin.Event.Update;
public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.MinPrice)
            .GreaterThan(0).WithMessage("Minimum price must be greater than 0.");

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.MinAge)
            .GreaterThanOrEqualTo((byte)0).WithMessage("Minimum age must be non-negative.");

        RuleFor(x => x.PlaceHallId)
            .GreaterThan(0).WithMessage("Place hall ID must be valid.");

        RuleFor(x => x.SubCategoryId)
            .GreaterThan(0).WithMessage("Subcategory ID must be valid.");
    }
}

