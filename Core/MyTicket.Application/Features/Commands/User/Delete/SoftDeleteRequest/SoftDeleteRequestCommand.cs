using MediatR;

namespace MyTicket.Application.Features.Commands.User.Delete.SoftDeleteRequest;
public class SoftDeleteRequestCommand : IRequest<bool>
{
    public string Email { get; set; }
}

