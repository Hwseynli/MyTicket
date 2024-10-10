using MediatR;

namespace MyTicket.Application.Features.Commands.User.Subscriber.Create;
public class CreateSubscriberCommand : IRequest<bool>
{
    public string EmailOrPhoneNumber { get; set; }
}

