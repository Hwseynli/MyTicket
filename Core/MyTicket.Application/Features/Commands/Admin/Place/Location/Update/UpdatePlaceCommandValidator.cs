using FluentValidation;
using MyTicket.Application.Features.Commands.Place.Location.Update;

namespace MyTicket.Application.Features.Commands.Admin.Place.Location.Update;
public class UpdatePlaceCommandValidator : AbstractValidator<UpdatePlaceCommand>
{
    public UpdatePlaceCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty()
            .GreaterThan(1).WithMessage("Id amount must be greater than 1.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Name must contain a maximum of 50 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Address must contain a maximum of 200 characters");
    }
}

