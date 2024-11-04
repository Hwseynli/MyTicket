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

    public void SetDetails(string uniqueCodde, decimal discountAmount, DiscountType discountType, byte expirationAfterDays, int usageLimit, bool isActive, int userId)
    {
        if ((discountAmount <= 0 || discountAmount > 100) && discountType == 0)
            throw new DomainException("Endirim faizi 0 və ya 100-dən böyük ola bilməz.");
        UniqueCode = uniqueCodde;
        DiscountType = discountType;
        DiscountAmount = discountAmount;
        ExpirationDate = DateTime.UtcNow.AddDays(expirationAfterDays);
        UsageLimit = usageLimit;
        IsActive = isActive;
        IsDeleted = false;
        DeletedDate = null;
        Orders = new List<Order>();
        UserPromoCodes = new List<UserPromoCode>();
        SetAuditDetails(userId);
    }

    public void AddUserForPromoCode(int userId)
    {
        UserPromoCode user = new UserPromoCode();
        user.SetDetails(userId, Id);
        UserPromoCodes.Add(user);
    }

    public void UpdateDetails(string code, decimal discountAmount, int usageLimit, int updatedById, DateTime expirationDate, byte expirationAfterDays = 0)
    {
        UniqueCode = code;
        DiscountAmount = discountAmount;
        ExpirationDate = expirationAfterDays > 0 ? DateTime.UtcNow.AddDays(expirationAfterDays) : expirationDate;
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

    public void SoftDelete(int userId)
    {
        Deactivate();
        IsDeleted = true;
        DeletedDate = DateTime.UtcNow.AddHours(4);
        SetEditFields(userId);
    }

    public decimal CalculateDiscount(decimal totalAmount, DiscountType? discountType = 0)
    {
        if (discountType == DiscountType.Percent)
            return totalAmount * (DiscountAmount / 100);
        else
            return DiscountAmount;
    }
}

