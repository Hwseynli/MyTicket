using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, bool>
{
    private readonly IUserRepository _userRepository;
    public VerifyOtpCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync
            (u => u.OtpCode != null && u.OtpCode == request.OtpCode &&
            u.OtpGeneratedTime != null &&
            u.OtpGeneratedTime.Value.AddMinutes(15) >= DateTime.UtcNow);

        if (user == null)
            throw new BadRequestException("Invalid OTP or OTP has expired.");

        return true;
    }
}

