using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Location.Create;
public class AddPlaceCommandValidator : AbstractValidator<AddPlaceCommand>
{
    private readonly IPlaceRepository _placeRepository;
    public AddPlaceCommandValidator(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository;
        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).MaximumLength(100)
            .WithMessage("min 3 max 100 simvoldan ibarət olmalıdır.")
            .MustAsync(async (name, cancellationToken) => await _placeRepository.IsPropertyUniqueAsync(x=>x.Name.ToLower(),name.Trim().ToLower()))
            .WithMessage("Bu başlıqda place yaradılıb istəyirsinizsə update edin ya da başqa ad ilə yaradın");
        RuleFor(p => p.Address).NotEmpty().MinimumLength(5).MaximumLength(200)
            .WithMessage("min 5 max 200 simvoldan ibarət olmalıdır.");
    }
}

