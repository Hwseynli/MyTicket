using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.Login;
public class CreateAuthorizationTokenCommandValidator : AbstractValidator<CreateAuthorizationTokenCommand>
{
    public CreateAuthorizationTokenCommandValidator()
    {
        RuleFor(command => command.EmailOrPhoneNumber)
                .NotEmpty().WithMessage(UIMessage.Required("Email or Phone number"));

        RuleFor(command => command.Password)
            .NotEmpty().WithMessage(UIMessage.Required("Password"))
            .MinimumLength(6).WithMessage(UIMessage.MinLength("Password", 6));
    }
}

