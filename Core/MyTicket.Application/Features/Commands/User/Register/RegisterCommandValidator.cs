using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.Register;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandValidator(IUserRepository userRepository) : base()
    {
        _userRepository = userRepository;

        RuleFor(command => command.FirstName).NotNull().MinimumLength(3).MaximumLength(30);
        RuleFor(command => command.LastName).NotNull().MinimumLength(3).MaximumLength(50);
        RuleFor(command => command.Password).MinimumLength(6).NotNull();
        RuleFor(command => command.Email).NotEmpty()
             .MustAsync(async (email, cancellation) =>
                 await _userRepository.IsPropertyUniqueAsync(u => u.Email, email))
             .WithMessage("Email artıq mövcuddur")
             .Must(x => x.Contains("@gmail.com")||x.Contains("@mail.ru"))
             .WithMessage("Sadəcə daxili mail olmalıdır");
        RuleFor(command => command.PhoneNumber).NotEmpty()
           .WithMessage("Phone number is required.")
           .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
           .MustAsync(async (phoneNumber, cancellation) =>
               await _userRepository.IsPropertyUniqueAsync(u => u.PhoneNumber, phoneNumber))
           .WithMessage("PhoneNumber artıq mövcuddur");
    }
}
