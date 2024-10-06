using MediatR;
using MyTicket.Application.Features.Commands.User.ViewModels;

namespace MyTicket.Application.Features.Commands.User.RefreshToken;
public class RefreshTokenCommand : IRequest<JwtTokenDto>
{
    public string RefreshToken { get; set; }
}

