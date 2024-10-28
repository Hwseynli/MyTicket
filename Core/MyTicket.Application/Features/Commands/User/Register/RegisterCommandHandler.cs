using MediatR;
using MyTicket.Application.Interfaces.IRepositories.Baskets;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IBasketRepository _basketRepository;

    public RegisterCommandHandler(IUserRepository userRepository, IBasketRepository basketRepository)
    {
        _userRepository = userRepository;
        _basketRepository = basketRepository;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.Users.User();
        user.SetDetailsForRegister(request.FirstName, request.LastName, request.PhoneNumber, request.Email, request.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.Commit(cancellationToken);

        var basket = new Domain.Entities.Baskets.Basket();
        basket.SetDetails(user.Id);
        await _basketRepository.AddAsync(basket);
        await _basketRepository.Commit(cancellationToken);
        return true;
    }
}