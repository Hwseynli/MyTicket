using FluentValidation;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommandValidator : AbstractValidator<AddPlaceHallCommand>
{
    public AddPlaceHallCommandValidator()
    {
        RuleFor(ph => ph.Name).NotEmpty().MinimumLength(3).MaximumLength(50);
        RuleFor(ph => ph.PlaceId).GreaterThan(0).WithMessage("PlaceId valid olmalıdır.");
    }
}

