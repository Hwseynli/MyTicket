using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.ForgotPassword.VerifyOtp;
public class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.OtpCode).NotEmpty().WithMessage(UIMessage.Required("Otp code"));
    }
}

