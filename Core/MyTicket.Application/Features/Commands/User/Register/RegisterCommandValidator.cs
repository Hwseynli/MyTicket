using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.Register;
public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandValidator(IUserRepository userRepository) : base()
    {
        _userRepository = userRepository;

        RuleFor(command => command.FirstName).NotNull().WithMessage(UIMessage.Required("First name"))
                .MinimumLength(3).WithMessage(UIMessage.MinLength("First name", 3))
                .MaximumLength(30).WithMessage(UIMessage.MaxLength("First name", 30));

        RuleFor(command => command.LastName).NotNull().WithMessage(UIMessage.Required("Last name"))
                .MinimumLength(3).WithMessage(UIMessage.MinLength("Last name", 3))
                .MaximumLength(50).WithMessage(UIMessage.MaxLength("Last name", 50));

        RuleFor(command => command.Password).NotNull().WithMessage(UIMessage.Required("Password"))
            .MinimumLength(6).WithMessage(UIMessage.MinLength("Password", 6));

        RuleFor(command => command.Email).NotEmpty().WithMessage(UIMessage.Required("Email"))
            .MustAsync(async (email, cancellation) => await _userRepository.IsPropertyUniqueAsync(u => u.Email, email)).WithMessage(UIMessage.UniqueProperty("Email"))
            .Must(x => x.Contains("@gmail.com") || x.Contains("@mail.ru")).WithMessage("Only internal emails are allowed");

        RuleFor(command => command.PhoneNumber).NotEmpty().WithMessage(UIMessage.Required("Phone number"))
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(UIMessage.ValidProperty("Phone number"))
            .MustAsync(async (phoneNumber, cancellation) => await _userRepository.IsPropertyUniqueAsync(u => u.PhoneNumber, phoneNumber)).WithMessage(UIMessage.UniqueProperty("Phone number"));
    }
}
