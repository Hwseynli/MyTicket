using MediatR;

namespace MyTicket.Application.Features.Commands.User.Delete.SoftDeleteConfirm;
public class SoftDeleteConfirmCommand : IRequest<bool>
{
    public string ConfirmToken { get; set; }
}

