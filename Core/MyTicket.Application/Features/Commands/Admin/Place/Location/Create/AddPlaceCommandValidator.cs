using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Place.Location.Create;
public class AddPlaceCommandValidator : AbstractValidator<AddPlaceCommand>
{
    private readonly IPlaceRepository _placeRepository;
    public AddPlaceCommandValidator(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository;
        RuleFor(p => p.Name).NotEmpty().WithMessage(UIMessage.Required("Name"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Name", 3))
            .MaximumLength(100).WithMessage(UIMessage.MaxLength("Name", 100))
            .MustAsync(async (name, cancellationToken) => await _placeRepository.IsPropertyUniqueAsync(x => x.Name.ToLower(), name.Trim().ToLower())).WithMessage(UIMessage.UniqueProperty("Name"));
        RuleFor(p => p.Address).NotEmpty().WithMessage(UIMessage.Required("Address"))
            .MinimumLength(5).WithMessage(UIMessage.MinLength("Address", 5))
            .MaximumLength(200).WithMessage(UIMessage.MaxLength("Address", 200));
    }
}

