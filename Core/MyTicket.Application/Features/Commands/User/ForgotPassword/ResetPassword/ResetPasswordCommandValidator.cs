using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.ForgotPassword.ResetPassword;
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
               .NotEmpty().WithMessage(UIMessage.Required("Email"))
               .EmailAddress().WithMessage(UIMessage.ValidProperty("Email"));

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(UIMessage.Required("New password"))
            .MinimumLength(6).WithMessage(UIMessage.MinLength("New password", 6));
    }
}
