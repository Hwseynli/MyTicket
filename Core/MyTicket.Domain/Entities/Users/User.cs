using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Baskets;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Domain.Entities.Favourites;
using MyTicket.Domain.Entities.Orders;
using MyTicket.Domain.Entities.PromoCodes;
using MyTicket.Domain.Entities.Ratings;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Users;
public class User : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Gender? Gender { get; private set; }
    public int RoleId { get; private set; }
    public Role Role { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime? Birthday { get; set; }
    public string PasswordHash { get; private set; }
    public List<Rating> Ratings { get; private set; }
    public List<Ticket> Tickets { get; private set; }
    public int? WishListId { get; private set; }
    public WishList? WishList { get; private set; }
    public DateTime LastPasswordChangeDateTime { get; private set; }
    public string? RefreshToken { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedTime { get; private set; }
    public bool Activated { get; private set; }
    public List<Order> Orders { get; private set; }
    public List<UserPromoCode> UserPromoCodes { get; private set; }
    //ForgotPassword üçün :
    public string? OtpCode { get; private set; }
    public DateTime? OtpGeneratedTime { get; private set; }

    public int? BasketId { get; private set; }
    public Basket? Basket { get; private set; }

    public void SetDetailsForRegister(string firstName, string lastName, string phoneNumber, string email, string password)
    {
        FirstName = firstName.Capitalize();
        LastName = lastName.Capitalize();
        PhoneNumber = phoneNumber;
        Email = email;
        Activated = false;
        IsDeleted = false;
        PasswordHash = PasswordHasher.HashPassword(password);
        RoleId = 2;
        Gender = Enums.Gender.Other;
        Ratings = new List<Rating>();
        WishList = new WishList();
        Orders = new List<Order>();
        UserPromoCodes = new List<UserPromoCode>();
    }
    public void SetForLogin(bool isActivated)
    {
        Activated = isActivated;
        IsDeleted = false;
        DeletedTime = null;
    }

    public void SetForSoftDelete()
    {
        Activated = false;
        IsDeleted = true;
        DeletedTime = DateTime.UtcNow;
        UpdateRefreshToken(null);
    }
    public void SetDetailsForUpdate(string firstName, string lastName, string phoneNumber, string email, Gender gender, DateTime? dateTime)
    {
        FirstName = firstName.Capitalize();
        LastName = lastName.Capitalize();
        Gender = gender;
        Birthday = dateTime;
        PhoneNumber = phoneNumber;
        Email = email;
        Activated = true;
        IsDeleted = false;
    }
    public void SetPasswordHash(string newPasswordHash)
    {
        if (PasswordHash != newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            LastPasswordChangeDateTime = DateTime.UtcNow.AddHours(4);
        }
    }
    public void UpdateRefreshToken(string? refreshToken)
    {
        RefreshToken = refreshToken;
    }

    //ForgotPassword üçün:
    public void UpdateOtp(string? otpCode)
    {
        OtpCode = otpCode;
        OtpGeneratedTime = DateTime.UtcNow.AddHours(4);
    }
    public void ResetPassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        LastPasswordChangeDateTime = DateTime.UtcNow.AddHours(4);
        OtpGeneratedTime = null;
        OtpCode = null;
    }

    public void UpdateRole(int roleId)
    {
        RoleId = roleId;
    }
}