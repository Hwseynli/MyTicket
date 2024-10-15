using FluentValidation;

namespace MyTicket.Application.Features.Commands.Event.Create;
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty();
        RuleFor(command => command.StartTime).LessThan(command => command.EndTime).WithMessage("Başlanğıc vaxtı son vaxtdan əvvəl olmalıdır.");
        RuleFor(command => command.MainImage).NotNull().WithMessage("Əsas şəkil əlavə olunmalıdır.");
        RuleFor(command => command.EventMedias).Must(x => x.Count > 0).WithMessage("Əlavə media faylları olmalıdır.");
    }
}

