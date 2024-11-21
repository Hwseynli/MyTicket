using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Infrastructure.BaseMessages;

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
        int userId = await _userManager.GetCurrentUserId();

        var place = await _placeRepository.GetAsync(p => p.Id == request.Id);
        if (place == null)
            throw new NotFoundException(UIMessage.NotFound("Place"));

        if (!await _placeRepository.IsPropertyUniqueAsync(x => x.Name, request.Name, place.Id))
            throw new BadRequestException(UIMessage.AlreadyExist("Place Name"));

        place.SetDetailsForUpdate(request.Name, request.Address, userId);
        await _placeRepository.Update(place);
        await _placeRepository.Commit(cancellationToken);

        return true;
    }
}

