using MyTicket.Application.Exceptions;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Queries.Admin.ViewModels;
public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string RoleName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime? LastPasswordChangeDateTime { get; private set; }
    public bool IsActivated { get; private set; }
    public bool IsDeleted { get; private set; }

    static UserDto CreateDto(int id, string firstName, string lastName, Gender? gender, DateTime? birthday, string phoneNumber, string email, string roleName, DateTime? lastPasswordChangeDateTime, bool isActivated, bool isDeleted)
    {
        return new UserDto
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender.ToString(),
            Birthday = birthday,
            PhoneNumber = phoneNumber,
            Email = email,
            RoleName = roleName,
            LastPasswordChangeDateTime = lastPasswordChangeDateTime,
            IsActivated = isActivated,
            IsDeleted = isDeleted,
        };
    }

    public static List<UserDto> CreateDtos(IEnumerable<Domain.Entities.Users.User> users)
    {
        if (users is null)
            throw new NotFoundException();
        List<UserDto> dtos = new List<UserDto>();
        foreach (var item in users)
        {
            var dto = CreateDto(
                id: item.Id,
                firstName: item.FirstName,
                lastName: item.LastName,
                gender: item.Gender,
                birthday: item.Birthday,
                roleName: item.Role.Name,
                phoneNumber: item.PhoneNumber,
                email: item.Email,
                lastPasswordChangeDateTime: item.LastPasswordChangeDateTime,
                isActivated: item.Activated,
                isDeleted: item.IsDeleted
                );
            dtos.Add(dto);
        }
        return dtos;
    }
}