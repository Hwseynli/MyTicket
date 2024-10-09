using MediatR;

namespace MyTicket.Application.Features.Commands.User.Logout;
public class LogoutUserCommand : IRequest<bool>
{
    public bool Disable { get; set; }
}