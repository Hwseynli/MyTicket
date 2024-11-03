using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.Delete.SoftDeleteRequest;
public class SoftDeleteRequestCommandHandler : IRequestHandler<SoftDeleteRequestCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailManager _emailManager;
    private readonly IUserManager _userManager;

    public SoftDeleteRequestCommandHandler(IUserRepository userRepository, IEmailManager emailManager, IUserManager userManager)
    {
        _userRepository = userRepository;
        _emailManager = emailManager;
        _userManager = userManager;
    }

    public async Task<bool> Handle(SoftDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetCurrentUser();
        if (user.Email != request.Email)
            throw new BadRequestException("Email doğru deyil");
        if (user.IsDeleted)
            return false;

        // User soft-delete edilir
        user.SetForSoftDelete();

        // 1 ay sonra tamamilə silinəcək barədə email göndərilir
        await _emailManager.SendEmailAsync(user.Email, "Account Deleted",
            "Your account has been soft deleted. It will be permanently deleted after 30 days unless you log in during this period.");

        _userRepository.Update(user);
        await _userRepository.Commit(cancellationToken);
        return true;
    }
}