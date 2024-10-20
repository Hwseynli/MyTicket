using MyTicket.Domain.Common;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Events;
using MyTicket.Infrastructure.Utils;

namespace MyTicket.Domain.Entities.Users;
public class User : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Gender? Gender { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public DateTime? Birthday { get; set; }
    public string PasswordHash { get; private set; }
    public List<Rating> Ratings { get; set; }
    public DateTime LastPasswordChangeDateTime { get; private set; }
    public string? RefreshToken { get; private set; }
    public string? ConfirmToken { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedTime { get; private set; }
    public bool Activated { get; private set; }
    //ForgotPassword üçün :
    public string? OtpCode { get; private set; }
    public DateTime? OtpGeneratedTime { get; private set; }

    public void SetDetailsForRegister(string firstName, string lastName, string phoneNumber, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        Activated = false;
        IsDeleted = false;
        PasswordHash = PasswordHasher.HashPassword(password);
        RoleId = 2;
        Ratings = new List<Rating>();
    }
    public void SetForLogin(bool isActivated)
    {
        Activated = isActivated;
        IsDeleted = false;
        DeletedTime = null;
    }
    public void SetConfirmToken(string token)
    {
        ConfirmToken = token;
    }
    public void SetForSoftDelete()
    {
        Activated = false;
        IsDeleted = true;
        DeletedTime = DateTime.UtcNow;
        UpdateRefreshToken(null);
        ConfirmToken = null;
    }
    public void SetDetailsForUpdate(string firstName, string lastName, string phoneNumber, string email, string passwordHash, Gender gender, DateTime? dateTime)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Birthday = dateTime;
        PhoneNumber = phoneNumber;
        Email = email;
        Activated = true;
        IsDeleted = false;
        PasswordHash = passwordHash;
    }
    public void SetPasswordHash(string newPasswordHash)
    {
        if (PasswordHash != newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            LastPasswordChangeDateTime = DateTime.UtcNow.AddHours(4);
        }
    }
    public void UpdateRefreshToken(string refreshToken)
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
}