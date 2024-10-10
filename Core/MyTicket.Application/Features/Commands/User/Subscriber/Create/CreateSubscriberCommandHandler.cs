using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.Subscriber.Create;
public class CreateSubscriberCommandHandler : IRequestHandler<CreateSubscriberCommand, bool>
{
    public readonly ISubscriberRepository _subscriberRepository;
    private readonly IEmailManager _emailManager;

    public CreateSubscriberCommandHandler(ISubscriberRepository subscriberRepository, IEmailManager emailManager)
    {
        _subscriberRepository = subscriberRepository;
        _emailManager = emailManager;
    }

    public async Task<bool> Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
    {
        bool exsist=false;
        bool isEmail = Helper.IsEmail(request.EmailOrPhoneNumber);

        // Emailin və ya telefon nömrəsinin unikal olub-olmadığını yoxla
        if (isEmail)
        {
            exsist =! await _subscriberRepository.IsPropertyUniqueAsync(x=>x.Email,request.EmailOrPhoneNumber);
        }
        else
        {
            exsist =! await _subscriberRepository.IsPropertyUniqueAsync(x => x.PhoneNumber, request.EmailOrPhoneNumber);
        }

        // Subscriber mövcuddursa, error at
        if (exsist)
            throw new ValidationException();

        // Yeni subscriber yaradılır
        var subscriber = new Domain.Entities.Users.Subscriber();
        
        // Xoş gəldiniz emailini göndər
        if (isEmail)
        {
            subscriber.SetDetail(email:request.EmailOrPhoneNumber);
            var subject = "Xoş gəldiniz!";
            var body = "Bizə abunə olduğunuz üçün təşəkkür edirik!";
            await _emailManager.SendEmailAsync(request.EmailOrPhoneNumber, subject, body);
        }
        else
        {
            subscriber.SetDetail(phoneNumber:request.EmailOrPhoneNumber);
        }

        return true;
    }
}