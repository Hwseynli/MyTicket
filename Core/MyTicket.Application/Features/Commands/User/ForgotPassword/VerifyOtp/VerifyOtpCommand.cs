using MediatR;

namespace MyTicket.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
public class VerifyOtpCommand : IRequest<bool>
{
    public string OtpCode { get; set; }
}