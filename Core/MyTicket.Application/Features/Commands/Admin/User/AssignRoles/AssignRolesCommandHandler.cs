using MediatR;
using MyTicket.Application.Interfaces.IRepositories;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Features.Commands.Admin.User.AssignRoles;
public class AssignRolesCommandHandler : IRequestHandler<AssignRolesCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<Role> _roleRepository;

    public AssignRolesCommandHandler(IUserRepository userRepository, IRepository<Role> roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<bool> Handle(AssignRolesCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(u => u.Id == request.UserId, "Role");
        if (user == null)
        {
            // Handle user not found case
            return false;
        }

        var role = await _roleRepository.GetAsync(r => r.Id == request.RoleId);
        if (role == null)
        {
            // Handle role not found case
            return false;
        }

        user.RoleId = request.RoleId;
        user.Role = role;

        _userRepository.Update(user);
        await _userRepository.Commit(cancellationToken);

        return true;
    }
}

