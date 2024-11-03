using MediatR;
using Microsoft.EntityFrameworkCore;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Orders;
using MyTicket.Application.Interfaces.IRepositories.Users;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Users;

namespace MyTicket.Application.Features.Commands.PromoCode.Create;
public class CreatePromoCodeCommandHandler : IRequestHandler<CreatePromoCodeCommand, bool>
{
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IUserManager _userManager;

    private readonly ISubscriberRepository _subscriberRepository;
    private readonly IEmailManager _emailManager;

    public CreatePromoCodeCommandHandler(IPromoCodeRepository promoCodeRepository, IUserManager userManager, ISubscriberRepository subscriberRepository, IEmailManager emailManager)
    {
        _promoCodeRepository = promoCodeRepository;
        _userManager = userManager;
        _subscriberRepository = subscriberRepository;
        _emailManager = emailManager;
    }

    public async Task<bool> Handle(CreatePromoCodeCommand request, CancellationToken cancellationToken)
    {
        var userId = await _userManager.GetCurrentUserId();

        var promoCode = new Domain.Entities.PromoCodes.PromoCode();
        promoCode.SetDetails(
            request.UniqueCode,
            request.DiscountAmount,
            request.DiscountType,
            request.ExpirationAfterDays,
            request.UsageLimit,
            request.IsActive,
            userId
        );

        try
        {
            await _promoCodeRepository.AddAsync(promoCode);
            await _promoCodeRepository.Commit(cancellationToken);
        }
        catch (DbUpdateException dbEx)
        {
            Console.WriteLine($"DbUpdateException: {dbEx.InnerException?.Message ?? dbEx.Message}");
            throw; // rethrow to retain original stack trace
        }

        if (promoCode.IsActive)
        {
            IEnumerable<Subscriber> subscribers = await _subscriberRepository.GetAllAsync(x => x.StringType == StringType.Email);
            await _emailManager.SendEmailForSubscribers(subscribers, "Great discount promo code", $"Expiry date: {promoCode.ExpirationDate}", $"PromoCode is : {promoCode.UniqueCode}");
        }
        return true;
    }
}

