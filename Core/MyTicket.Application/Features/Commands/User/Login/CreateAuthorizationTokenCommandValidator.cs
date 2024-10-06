using FluentValidation;

namespace MyTicket.Application.Features.Commands.User.Login;
public class CreateAuthorizationTokenCommandValidator : AbstractValidator<CreateAuthorizationTokenCommand>
{
    public CreateAuthorizationTokenCommandValidator()
    {
        // Email və ya Telefon nömrəsinin doldurulmasını yoxla
        RuleFor(command => command.EmailOrPhoneNumber)
            .NotEmpty().WithMessage("Email or Phone number is required.");

        // Şifrə yoxlanışı - minimum uzunluq 6
        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
    }
}

