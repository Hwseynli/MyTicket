using FluentValidation;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.UpdateUser;
public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserManager _userManager;

    public UpdateUserCommandValidator(IUserRepository userRepository, IUserManager userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;

        RuleFor(command => command.FirstName).NotEmpty().WithMessage(UIMessage.Required("First name"))
                .MinimumLength(3).WithMessage(UIMessage.MinLength("First name", 3))
                .MaximumLength(30).WithMessage(UIMessage.MaxLength("First name", 30));

        RuleFor(command => command.LastName).NotEmpty().WithMessage(UIMessage.Required("Last name"))
            .MinimumLength(3).WithMessage(UIMessage.MinLength("Last name", 3))
            .MaximumLength(50).WithMessage(UIMessage.MaxLength("Last name", 50));

        RuleFor(command => command.Gender)
            .IsInEnum().WithMessage(UIMessage.ValidProperty("Gender"));

        RuleFor(command => command.Birthday)
            .LessThan(DateTime.Now).WithMessage(UIMessage.ValidProperty("Birthday"));

        RuleFor(command => command.Email).NotEmpty().WithMessage(UIMessage.Required("Email"))
            .EmailAddress().WithMessage(UIMessage.ValidProperty("Email"))
            .MustAsync(async (email, cancellation) =>
                await _userRepository.IsPropertyUniqueAsync(u => u.Email, email, await _userManager.GetCurrentUserId()))
            .WithMessage(UIMessage.UniqueProperty("Email"));

        RuleFor(command => command.PhoneNumber).NotEmpty().WithMessage(UIMessage.Required("Phone number"))
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage(UIMessage.ValidProperty("Phone number"))
            .MustAsync(async (phone, cancellation) =>
                await _userRepository.IsPropertyUniqueAsync(u => u.PhoneNumber, phone, await _userManager.GetCurrentUserId()))
            .WithMessage(UIMessage.UniqueProperty("Phone number"));

        RuleFor(command => command.Password).NotEmpty().WithMessage(UIMessage.Required("Password"))
            .MinimumLength(6).WithMessage(UIMessage.MinLength("Password", 6));
    }
}