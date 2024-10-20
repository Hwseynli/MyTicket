using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Application.Features.Commands.User.Subscriber.Create;
public class CreateSubscriberCommandHandler : IRequestHandler<CreateSubscriberCommand, bool>
{
    public readonly ISubscriberRepository _subscriberRepository;
    private readonly IEmailManager _emailManager;
    private readonly ISmsManager _smsManager;

    public CreateSubscriberCommandHandler(ISubscriberRepository subscriberRepository, IEmailManager emailManager, ISmsManager smsManager)
    {
        _subscriberRepository = subscriberRepository;
        _emailManager = emailManager;
        _smsManager = smsManager;
    }

    public async Task<bool> Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
    {
        bool exsist=false;
        bool isEmail = Helper.IsEmail(request.EmailOrPhoneNumber);
        bool isPhoneNumber = Helper.IsPhoneNumber(request.EmailOrPhoneNumber);

        // Emailin və ya telefon nömrəsinin unikal olub-olmadığını yoxla
            exsist =! await _subscriberRepository.IsPropertyUniqueAsync(x=>x.EmailOrPhoneNumber,request.EmailOrPhoneNumber);

        // Subscriber mövcuddursa, error at
        if (exsist)
            throw new ValidationException();

        // Yeni subscriber yaradılır
        var subscriber = new Domain.Entities.Users.Subscriber();
        
        // Xoş gəldiniz emailini göndər
        if (isEmail)
        {
            subscriber.SetDetail(request.EmailOrPhoneNumber, (StringType)1);
            await _subscriberRepository.AddAsync(subscriber);
            await _subscriberRepository.Commit(cancellationToken);
            var subject = "Xoş gəldiniz!";
            var body = "Bizə abunə olduğunuz üçün təşəkkür edirik!";
            await _emailManager.SendEmailAsync(request.EmailOrPhoneNumber, subject, body);
        }
        else if(isPhoneNumber)
        {
            subscriber.SetDetail(request.EmailOrPhoneNumber,0);
            await _subscriberRepository.AddAsync(subscriber);
            await _subscriberRepository.Commit(cancellationToken);
            //// SMS göndəririk
            //var subject = "Xoş gəldiniz!";
            //var body = "Bizə abunə olduğunuz üçün təşəkkür edirik!";
            //await _smsManager.SendSmsAsync(request.EmailOrPhoneNumber, subject, body);
        }
        else
            throw new ValidationException();

        return true;
    }
}