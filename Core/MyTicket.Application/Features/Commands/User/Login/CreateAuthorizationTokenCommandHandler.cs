using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Commands.User.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.BaseMessages;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.Login;
public class CreateAuthorizationTokenCommandHandler : IRequestHandler<CreateAuthorizationTokenCommand, JwtTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;
    public CreateAuthorizationTokenCommandHandler(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<JwtTokenDto> Handle(CreateAuthorizationTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetAsync(x => x.Email.ToLower() == request.EmailOrPhoneNumber.ToLower()
                                                          || x.PhoneNumber == request.EmailOrPhoneNumber,"Role");

        if (user == null)
            throw new UnAuthorizedException(UIMessage.Invalid);

        // PasswordHasher is used to verify the password
        if (user.PasswordHash != PasswordHasher.HashPassword(request.Password))
        throw new UnAuthorizedException(UIMessage.Invalid);

        user.SetForLogin(true);

        // Refresh token is generated
        var random = Generator.GenerateRandomNumber();
        var refreshToken = $"{random}_{user.Id}_{DateTime.UtcNow.AddDays(20)}";
        user.UpdateRefreshToken(refreshToken);

        // Creating a JWT token
        (string token, DateTime expireAt) = _userManager.GenerateTJwtToken(user);
        await _userRepository.Commit(cancellationToken);

        return new JwtTokenDto
        {
            ExpireAt = expireAt,
            RefreshToken = refreshToken,
            Token = token
        };
    }       
}