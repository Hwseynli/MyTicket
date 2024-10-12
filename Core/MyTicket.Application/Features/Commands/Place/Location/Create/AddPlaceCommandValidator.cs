using FluentValidation;

namespace MyTicket.Application.Features.Commands.Place.Location.Create;
public class AddPlaceCommandValidator : AbstractValidator<AddPlaceCommand>
{
    public AddPlaceCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(p => p.Address).NotEmpty().MinimumLength(5).MaximumLength(200);
    }
}

