using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Location.Update;
public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, bool>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUserManager _userManager;

    public UpdatePlaceCommandHandler(IPlaceRepository placeRepository, IUserManager userManager)
    {
        _placeRepository = placeRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _placeRepository.GetAsync(p => p.Id == request.Id);

        if (place == null)
            throw new NotFoundException("Place not found.");

        request.Name = request.Name ?? place.Name;
        request.Address = request.Address ?? place.Address;

        place.SetDetailsForUpdate(request.Name, request.Address, _userManager.GetCurrentUserId());
        await _placeRepository.Update(place);
        await _placeRepository.Commit(cancellationToken);

        return true;
    }
}

