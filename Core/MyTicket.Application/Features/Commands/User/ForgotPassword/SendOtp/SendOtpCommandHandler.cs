using FluentValidation;
using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.ForgotPassword.SendOtp
{
    public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailManager _emailManager;
        private readonly IValidator<SendOtpCommand> _validator;

        public SendOtpCommandHandler(IUserRepository userRepository, IEmailManager emailManager, IValidator<SendOtpCommand> validator)
        {
            _userRepository = userRepository;
            _emailManager = emailManager;
            _validator = validator;
        }

        public async Task<bool> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new Exceptions.ValidationException(validationResult.Errors);

            var user = await _userRepository.GetAsync(u => u.Email == request.Email);
            if (user == null)
                throw new NotFoundException("User not found.");

            var otpCode = Generator.GenerateOtp(); // OTP yaradılır
            user.UpdateOtp(otpCode); // OTP user obyektində saxlanılır (məsələn, baza üçün)
            await _userRepository.Commit(cancellationToken);

            await _emailManager.SendOtpAsync(user.Email, otpCode); // OTP email ilə göndərilir
            return true;
        }
    }
}