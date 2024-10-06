using MediatR;
using MyTicket.Application.Interfaces.IRepositories;

namespace MyTicket.Application.Features.Commands.User.Register;
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.Users.User();

        user.SetDetailsForRegister(request.FirstName, request.LastName, request.PhoneNumber, request.Email, request.Password, request.Gender, request.Birthday);

        await _userRepository.AddAsync(user);
        await _userRepository.Commit(cancellationToken);
        return true;
    }
}