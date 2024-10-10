using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.Delete.SoftDeleteConfirm;
public class SoftDeleteConfirmCommandHandler : IRequestHandler<SoftDeleteConfirmCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailManager _emailManager;

    public SoftDeleteConfirmCommandHandler(IUserRepository userRepository, IEmailManager emailManager)
    {
        _userRepository = userRepository;
        _emailManager = emailManager;
    }

    public async Task<bool> Handle(SoftDeleteConfirmCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.ConfirmToken == request.ConfirmToken);
        if (user == null || user.IsDeleted)
            return false;

        // User soft-delete edilir
        user.SetForSoftDelete();

        // 1 ay sonra tamamilə silinəcək barədə email göndərilir
        await _emailManager.SendEmailAsync(user.Email, "Account Deleted",
            "Your account has been soft deleted. It will be permanently deleted after 30 days unless you log in during this period.");

        await _userRepository.Commit(cancellationToken);
        return true;
    }
}