using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.ForgotPassword.SendOtp;
public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(UIMessage.Required("Email"))
            .EmailAddress().WithMessage(UIMessage.ValidProperty("Email"));
    }
}