using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Users;

namespace MyTicket.Application.Features.Commands.User.Subscriber.Create;
public class CreateSubscriberCommandValidator : AbstractValidator<CreateSubscriberCommand>
{
    private readonly ISubscriberRepository _subscriberRepository;
    public CreateSubscriberCommandValidator(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;

        RuleFor(x => x.EmailOrPhoneNumber)
            .NotEmpty().WithMessage("Email or phone number is required.")
            .Must(_subscriberRepository.IsValidEmailOrPhoneNumber).WithMessage("Invalid email or phone number format.");
    }
}

