using MyTicket.Domain.Entities.PromoCodes;

namespace MyTicket.Application.Interfaces.IManagers;
public interface IOrderManager
{
    byte[] GenerateReceipt(Domain.Entities.Orders.Order order, decimal discountAmount);
    Task<PromoCode> GetPromoCodeByIdAsync(int promoCodeId, int userId);
    Task PaymentForStripe(string token_visa, string email, string firstName, string lastName, string phoneNumber, decimal orderTotalAmount);
    StringContent PaymentForKapital(decimal totalAmount, string orderCode);
}

