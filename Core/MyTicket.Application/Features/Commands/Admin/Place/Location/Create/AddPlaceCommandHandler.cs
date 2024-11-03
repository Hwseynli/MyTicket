using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Location.Create;
public class AddPlaceCommandHandler : IRequestHandler<AddPlaceCommand, bool>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUserManager _userManager;

    public AddPlaceCommandHandler(IPlaceRepository placeRepository, IUserManager userManager)
    {
        _placeRepository = placeRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(AddPlaceCommand request, CancellationToken cancellationToken)
    {
        int userId = await _userManager.GetCurrentUserId();

        var place = new Domain.Entities.Places.Place();
        place.SetDetails(request.Name, request.Address, userId);

        await _placeRepository.AddAsync(place);
        await _placeRepository.Commit(cancellationToken);
        return true;
    }
}
