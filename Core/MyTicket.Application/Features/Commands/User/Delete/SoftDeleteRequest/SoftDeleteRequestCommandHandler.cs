using MediatR;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.Delete.SoftDeleteRequest;
public class SoftDeleteRequestCommandHandler : IRequestHandler<SoftDeleteRequestCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailManager _emailManager;

    public SoftDeleteRequestCommandHandler(IUserRepository userRepository, IEmailManager emailManager)
    {
        _userRepository = userRepository;
        _emailManager = emailManager;
    }

    public async Task<bool> Handle(SoftDeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Email == request.Email);
        if (user == null || user.IsDeleted)
            return false;

        // ConfirmToken yaradılır
        var confirmToken = Generator.GenerateConfirmToken();
        user.SetConfirmToken(confirmToken);

        // Token email vasitəsilə göndərilir
        var confirmationLink = $"https://yourapi.com/confirm-soft-delete?token={confirmToken}";
        await _emailManager.SendEmailAsync(user.Email, "Confirm Soft Delete",
            $"Please confirm your account deletion by clicking this link: <a href='{confirmationLink}'>Confirm Deletion</a>");

        await _userRepository.Commit(cancellationToken);
        return true;
    }
}