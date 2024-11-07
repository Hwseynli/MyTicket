using FluentValidation;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Application.Features.Commands.User.Subscriber.Create;
public class CreateSubscriberCommandValidator : AbstractValidator<CreateSubscriberCommand>
{
    private readonly ISubscriberRepository _subscriberRepository;
    public CreateSubscriberCommandValidator(ISubscriberRepository subscriberRepository)
    {
        _subscriberRepository = subscriberRepository;

        RuleFor(x => x.EmailOrPhoneNumber).NotEmpty().WithMessage(UIMessage.Required("Email or phone number"))
                 .Must(_subscriberRepository.IsValidEmailOrPhoneNumber).WithMessage(UIMessage.ValidProperty("Email or phone number"))
                 .MustAsync(async (emailOrPhone, cancellation) => await _subscriberRepository.IsPropertyUniqueAsync(u => u.EmailOrPhoneNumber, emailOrPhone)).WithMessage(UIMessage.UniqueProperty("Email or phone number"));
    }
}

