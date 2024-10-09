using FluentValidation;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public UpdateUserCommandValidator(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;

        RuleFor(command => command.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(3).MaximumLength(30);

        RuleFor(command => command.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(3).MaximumLength(50);

        RuleFor(command => command.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .IsInEnum().WithMessage("Gender is invalid.");

        RuleFor(command => command.Birthday)
            .LessThan(DateTime.Now).WithMessage("Birthday must be in the past.");

        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MustAsync(async (email, cancellation) =>
                await _userRepository.IsPropertyUniqueAsync(u => u.Email, email, _userManager.GetCurrentUserId()))
            .WithMessage("Email already exists.");

        RuleFor(command => command.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format.")
            .MustAsync(async (phoneNumber, cancellation) =>
                await _userRepository.IsPropertyUniqueAsync(u => u.PhoneNumber, phoneNumber, _userManager.GetCurrentUserId()))
                .WithMessage("Phone number already exists.");

        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}