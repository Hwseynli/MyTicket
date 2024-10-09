using MediatR;

namespace MyTicket.Application.Features.Commands.Admin.User.AssignRoles;
public class AssignRolesCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}

