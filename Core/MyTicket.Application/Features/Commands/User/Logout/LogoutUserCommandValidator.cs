using FluentValidation;

namespace MyTicket.Application.Features.Commands.User.Logout;
public class LogoutUserCommandValidator : AbstractValidator<LogoutUserCommand>
{
    public LogoutUserCommandValidator()
    {
        RuleFor(command => command.Disable).NotEmpty();
    }
}
