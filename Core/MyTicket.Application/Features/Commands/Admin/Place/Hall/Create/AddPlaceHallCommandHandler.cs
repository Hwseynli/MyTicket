using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create;
public class AddPlaceHallCommandHandler : IRequestHandler<AddPlaceHallCommand, bool>
{
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly IUserManager _userManager;

    public AddPlaceHallCommandHandler(IPlaceHallRepository placeHallRepository, IUserManager userManager)
    {
        _placeHallRepository = placeHallRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(AddPlaceHallCommand request, CancellationToken cancellationToken)
    {
        var placeHall = new Domain.Entities.Places.PlaceHall();
        placeHall.SetDetails(request.Name, request.PlaceId, _userManager.GetCurrentUserId());

        await _placeHallRepository.AddAsync(placeHall);
        await _placeHallRepository.Commit(cancellationToken);
        return true;
    }
}

