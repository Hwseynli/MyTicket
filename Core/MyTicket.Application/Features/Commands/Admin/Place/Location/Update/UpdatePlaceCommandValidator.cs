using FluentValidation;
using MyTicket.Application.Features.Commands.Place.Location.Update;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.Place.Location.Update;
public class UpdatePlaceCommandValidator : AbstractValidator<UpdatePlaceCommand>
{
    public UpdatePlaceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(UIMessage.Required("Id"))
            .GreaterThan(1).WithMessage(UIMessage.GreaterThanZero("Id"));

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(UIMessage.Required("Name"))
            .MaximumLength(100).WithMessage(UIMessage.MaxLength("Name", 100));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage(UIMessage.Required("Address"))
            .MaximumLength(200).WithMessage(UIMessage.MaxLength("Address", 200));
    }
}

