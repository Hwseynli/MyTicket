using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Orders;
using MyTicket.Domain.Entities.Users;
using MyTicket.Domain.Exceptions;

namespace MyTicket.Domain.Entities.PromoCodes;
public class PromoCode : Editable<User>
{
    public string UniqueCode { get; private set; }//regex
    public decimal DiscountAmount { get; private set; }//miqdar 
    public DiscountType DiscountType { get; private set; }//faizoramount
    public DateTime ExpirationDate { get; private set; } // Promokodun bitmə tarixi
    public int UsageLimit { get; private set; } // Ümumi istifadə limiti
    public bool IsActive { get; private set; }
    public bool? IsDeleted { get; private set; }
    public DateTime? DeletedDate { get; private set; }
    public List<Order> Orders { get; private set; }
    public List<UserPromoCode> UserPromoCodes { get; private set; } // İstifadəçi ilə əlaqəsi

    public void SetDetails(string uniqueCodde, decimal discountAmount, DiscountType discountType, DateTime expireTime, int usageLimit, bool isActive, int userId)
    {
        if ((discountAmount <= 0 || discountAmount > 100) && discountType == 0)
            throw new DomainException("Endirim faizi 0 və ya 100-dən böyük ola bilməz.");
        UniqueCode = uniqueCodde;
        DiscountType = discountType;
        DiscountAmount = discountAmount;
        ExpirationDate = expireTime;
        UsageLimit = usageLimit;
        IsActive = isActive;
        IsDeleted = false;
        DeletedDate = null;
        UserPromoCodes = new List<UserPromoCode>();
        Orders = new List<Order>();
        SetAuditDetails(userId);
    }

    public void UpdateDetails(string code, decimal discountAmount, DateTime expiryDate, int usageLimit, int updatedById)
    {
        UniqueCode = code;
        DiscountAmount = discountAmount;
        ExpirationDate = expiryDate;
        UsageLimit = usageLimit;
        SetEditFields(updatedById);
    }

    public bool IsValid()
    {
        return IsActive && DateTime.UtcNow <= ExpirationDate;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public decimal ApplyDiscount(decimal totalAmount, DiscountType? discountType = 0)
    {
        if (discountType == DiscountType.Percent)
            return totalAmount * (DiscountAmount / 100);
        else
            return DiscountAmount;
    }
}