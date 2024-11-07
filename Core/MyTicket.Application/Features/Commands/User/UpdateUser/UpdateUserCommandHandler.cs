using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.BaseMessages;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.UpdateUser;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetCurrentUser();

        if (user.PasswordHash != PasswordHasher.HashPassword(request.Password))
            throw new UnAuthorizedException(UIMessage.ValidProperty("Password"));

        // İstifadəçinin məlumatlarını yenilə
        user.SetDetailsForUpdate(request.FirstName, request.LastName, request.PhoneNumber, request.Email, request.Gender, request.Birthday);

        // Məlumatları yadda saxla
        await _userRepository.Update(user);
        await _userRepository.Commit(cancellationToken);

        return true;
    }
}
