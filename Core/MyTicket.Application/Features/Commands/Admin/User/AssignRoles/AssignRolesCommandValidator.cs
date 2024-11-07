using FluentValidation;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.User.AssignRoles;
public class AssignRolesCommandValidator : AbstractValidator<AssignRolesCommand>
{
    public AssignRolesCommandValidator()
    {
        RuleFor(command => command.UserId).NotEmpty().WithMessage(UIMessage.Required("user id"))
            .GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("User Id"));
        RuleFor(command => command.RoleId).NotEmpty().WithMessage(UIMessage.Required("role id")).GreaterThan(0).WithMessage(UIMessage.GreaterThanZero("Role Id"));
    }
}

