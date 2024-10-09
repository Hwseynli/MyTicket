using FluentValidation;

namespace MyTicket.Application.Features.Commands.Admin.User.AssignRoles;
public class AssignRolesCommandValidator : AbstractValidator<AssignRolesCommand>
{
    public AssignRolesCommandValidator()
    {
        RuleFor(command => command.UserId).GreaterThan(0).WithMessage("UserId is required.");
        RuleFor(command => command.RoleId).GreaterThan(0).WithMessage("RoleId is required.");
    }
}

