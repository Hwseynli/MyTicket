using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Commands.Place.Hall.Update;
public class UpdatePlaceHallCommandHandler : IRequestHandler<UpdatePlaceHallCommand, bool>
{
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly IUserManager _userManager;

    public UpdatePlaceHallCommandHandler(IPlaceHallRepository placeHallRepository, IUserManager userManager)
    {
        _placeHallRepository = placeHallRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdatePlaceHallCommand request, CancellationToken cancellationToken)
    {
        var placeHall = await _placeHallRepository.GetAsync(ph => ph.Id == request.Id);

        if (placeHall == null)
            throw new NotFoundException("Place Hall not found.");

        request.HallName = request.HallName ?? placeHall.Name;

        placeHall.SetEditFields(_userManager.GetCurrentUserId());

        await _placeHallRepository.Update(placeHall);
        await _placeHallRepository.Commit(cancellationToken);

        return true;
    }
}

