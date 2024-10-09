using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Commands.User.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, JwtTokenDto>
{
    private readonly IUserManager _userManager;
    private readonly IUserRepository _userRepository;
    public RefreshTokenCommandHandler(IUserManager userManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<JwtTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = request.RefreshToken.Split("_");
        var user = await _userRepository.GetAsync(p => p.RefreshToken == request.RefreshToken);
        if (user == null
            || !user.Activated
            || user.IsDeleted)
            throw new UnAuthorizedException("Invalid credentials");

        if (Convert.ToDateTime(refreshToken[2]) < DateTime.UtcNow)
            throw new UnAuthorizedException("token is expiredd");
        (string token, DateTime expire) = _userManager.GenerateTJwtToken(user);
        return new JwtTokenDto
        {
            ExpireAt = expire,
            Token = token,
            RefreshToken = request.RefreshToken
        };
    }
}

