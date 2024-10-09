using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IRepositories;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.ForgotPassword.ResetPassword;
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public ResetPasswordCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.OtpCode != null && u.Email == request.Email && u.OtpGeneratedTime != null);

        if (user == null)
            throw new BadRequestException("Invalid OTP or OTP has expired.");

        user.ResetPassword(PasswordHasher.HashPassword(request.NewPassword));
        user.UpdateOtp(null); // OTP-i sil
        await _userRepository.Commit(cancellationToken);

        return true;
    }
}
