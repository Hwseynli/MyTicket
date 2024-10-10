using System.Security.Cryptography;
using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Features.Commands.User.ViewModels;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.Login
{
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
                throw new UnAuthorizedException("Invalid credentials");

            // Şifrəni yoxlamaq üçün PasswordHasher istifadə edilir
            if (user.PasswordHash != PasswordHasher.HashPassword(request.Password))
                throw new UnAuthorizedException("Invalid password");

            user.SetForLoginOrLogout(true,false);
            // Refresh token yaradılır
            var random = GenerateRandomNumber();
            var refreshToken = $"{random}_{user.Id}_{DateTime.UtcNow.AddDays(20)}";
            user.UpdateRefreshToken(refreshToken);

            // JWT token yaradılır
            (string token, DateTime expireAt) = _userManager.GenerateTJwtToken(user);
            await _userRepository.Commit(cancellationToken);

            // JWT Token və Refresh Token qaytarılır
            return new JwtTokenDto
            {
                ExpireAt = expireAt,
                RefreshToken = refreshToken,
                Token = token
            };

        }

        // Random rəqəm yaratmaq üçün funksiya
        private object GenerateRandomNumber()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}