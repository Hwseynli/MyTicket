using FluentValidation;
using MyTicket.Infrastructure.Extensions;

namespace MyTicket.Application.Features.Commands.Event.Update;
public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator()
    {
        RuleFor(command => command.Id).GreaterThan(0).WithMessage("Event ID must be greater than zero.");
        RuleFor(command => command.Name).NotEmpty().WithMessage("Event name cannot be empty.");
        RuleFor(command => command.StartTime).LessThan(command => command.EndTime).WithMessage("Start time must be before end time.");
        RuleFor(command => command.MainImage).NotNull().WithMessage("Main image is required.");

        // Validate the main image if provided
        When(command => command.MainImage != null, () =>
        {
            RuleFor(command => command.MainImage).Must(file => file.IsImage()).WithMessage("Yalnız şəkil fayllarına icazə verilir.");
        });
    }
}

