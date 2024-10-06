using MediatR;
using MyTicket.Application.Features.Commands.User.ViewModels;

namespace MyTicket.Application.Features.Commands.User.Login;
public class CreateAuthorizationTokenCommand : IRequest<JwtTokenDto>
{
    public string EmailOrPhoneNumber { get; set; }
    public string Password { get; set; }
}