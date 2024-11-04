using MediatR;
using MyTicket.Application.Exceptions;
using MyTicket.Application.Interfaces.IManagers;
using MyTicket.Application.Interfaces.IRepositories.Orders;

namespace MyTicket.Application.Features.Commands.PromoCode.Delete;
public class DeletePromoCodeCommandHandler : IRequestHandler<DeletePromoCodeCommand, bool>
{
    private readonly IPromoCodeRepository _promoCodeRepository;
    private readonly IUserManager _userManager;

    public DeletePromoCodeCommandHandler(IPromoCodeRepository promoCodeRepository, IUserManager userManager)
    {
        _promoCodeRepository = promoCodeRepository;
        _userManager = userManager;
    }

    public async Task<bool> Handle(DeletePromoCodeCommand request, CancellationToken cancellationToken)
    {
        var userId = await _userManager.GetCurrentUserId();

        var promoCode = await _promoCodeRepository.GetAsync(x => x.Id == request.Id);

        if (promoCode == null)
            throw new NotFoundException("Promo code not found.");

        promoCode.SoftDelete(userId);
        await _promoCodeRepository.Update(promoCode);
        await _promoCodeRepository.Commit(cancellationToken);
        return true;
    }
}

