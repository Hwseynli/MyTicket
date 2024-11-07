using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.Admin.User.AssignRoles;
public class AssignRolesCommandHandler : IRequestHandler<AssignRolesCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;
    private readonly IRoleRepository _roleRepository;

    public AssignRolesCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(AssignRolesCommand request, CancellationToken cancellationToken)
    {
        var admin = await _userManager.GetCurrentUser();
        if (admin.RoleId!=1)
        {
            throw new UnAuthorizedException(UIMessage.NotAccess());
        }
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId&&u.Id!=admin.Id&&u.RoleId!=1);
        if (user == null)
        {
            throw new NotFoundException(UIMessage.NotFound("User"));
        }

        var role = await _roleRepository.GetAsync(r => r.Id == request.RoleId);
        if (role == null)
        {
            throw new NotFoundException(UIMessage.NotFound("Role"));
        }

        user.UpdateRole(role.Id);
        await _userRepository.Update(user);
        await _userRepository.Commit(cancellationToken);

        return true;
    }
}

