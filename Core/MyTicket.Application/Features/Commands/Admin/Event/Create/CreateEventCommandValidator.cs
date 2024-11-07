using FluentValidation;
using MyTicket.Application.Features.Commands.Admin.Event.ViewModels;
using MyTicket.Application.Interfaces.IRepositories.Events;
using MyTicket.Infrastructure.BaseMessages;
using MyTicket.Infrastructure.Extensions;

namespace MyTicket.Application.Features.Commands.Admin.Event.Create;
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandValidator(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;

        RuleFor(x => x.Title)
              .NotEmpty().WithMessage(UIMessage.NotEmpty("Event title"))
              .MustAsync(async (title, cancellationToken) => await _eventRepository.IsPropertyUniqueAsync(x => x.Title, title))
                .WithMessage(UIMessage.UniqueProperty("Event title"))
              .MaximumLength(100).WithMessage(UIMessage.MaxLength("Event title", 100));

        RuleFor(x => x.MinPrice)
           .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Price"));

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage(UIMessage.NotEmpty("Event start time"))
            .GreaterThan(DateTime.Now).WithMessage("Event start time cannot be in the past.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage(UIMessage.NotEmpty("Event end time"))
            .GreaterThan(x => x.StartTime).WithMessage(UIMessage.GreaterThan("Event end time", "Event start time"));

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage(UIMessage.NotEmpty("Event description"))
            .MaximumLength(500).WithMessage(UIMessage.MaxLength("Event description", 500));

        RuleFor(x => x.MinAge)
            .NotEmpty().WithMessage(UIMessage.NotEmpty("Event minimum age"));

        RuleFor(x => x.PlaceHallId)
            .GreaterThan(0).WithMessage(UIMessage.ValidProperty("Place Hall ID"));

        RuleFor(x => x.SubCategoryId)
            .GreaterThan(0).WithMessage(UIMessage.ValidProperty("Subcategory ID"));

        RuleForEach(x => x.EventMediaModels)
            .NotNull().WithMessage(UIMessage.NotEmpty("Event media"))
            .SetValidator(new EventMediaModelValidator());
    }
}

public class EventMediaModelValidator : AbstractValidator<EventMediaModel>
{
    public EventMediaModelValidator()
    {
        RuleFor(x => x.MainImage)
          .NotNull().WithMessage(UIMessage.NotEmpty("Main image"))
          .Must((mainImage) => mainImage.IsImage()).WithMessage(UIMessage.InvalidImage("Main image"));
    }
}