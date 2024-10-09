using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories;

namespace MyTicket.Application.Features.Commands.User.Logout;
public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public LogoutUserCommandHandler(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        if (!request.Disable)
            return false;
        // İstifadəçini tap
        var user = await _userRepository.GetAsync(u=>u.Id==_userManager.GetCurrentUserId()&&u.Activated);
        if (user == null)
            throw new UnAuthorizedException();

        user.UpdateRefreshToken(null); // Refresh tokenini sıfırla, sessiyanı bitir
        user.SetForLogout();
        await _userRepository.Commit(cancellationToken); // Dəyişiklikləri saxla

        return true;
    }
}