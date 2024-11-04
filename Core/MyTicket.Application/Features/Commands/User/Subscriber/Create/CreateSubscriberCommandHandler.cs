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
    public readonly IUserRepository _userRepository;
    private readonly IEmailManager _emailManager;
    private readonly ISmsManager _smsManager;

    public CreateSubscriberCommandHandler(ISubscriberRepository subscriberRepository, IEmailManager emailManager, ISmsManager smsManager, IUserRepository userRepository)
    {
        _subscriberRepository = subscriberRepository;
        _emailManager = emailManager;
        _smsManager = smsManager;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(CreateSubscriberCommand request, CancellationToken cancellationToken)
    {
        bool isEmail = Helper.IsEmail(request.EmailOrPhoneNumber);
        bool isPhoneNumber = Helper.IsPhoneNumber(request.EmailOrPhoneNumber);

        // Yeni subscriber yaradılır
        var subscriber = new Domain.Entities.Users.Subscriber();
        
        // Xoş gəldiniz emailini göndər
        if (isEmail)
        {
            if (await _userRepository.GetAsync(x => x.Email == request.EmailOrPhoneNumber && x.RoleId == 1) != null)
                throw new UnAuthorizedException("Sən adminsən və abunə ola bilmərsən");
            subscriber.SetDetail(request.EmailOrPhoneNumber, (StringType)1);
            await _subscriberRepository.AddAsync(subscriber);
            await _subscriberRepository.Commit(cancellationToken);
            var subject = "Xoş gəldiniz!";
            var body = "Bizə abunə olduğunuz üçün təşəkkür edirik!";
            await _emailManager.SendEmailAsync(request.EmailOrPhoneNumber, subject, body);
        }
        else if(isPhoneNumber)
        {
            if (await _userRepository.GetAsync(x => x.PhoneNumber == request.EmailOrPhoneNumber && x.RoleId == 1) != null)
                throw new UnAuthorizedException("Sən adminsən və abunə ola bilmərsən");
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