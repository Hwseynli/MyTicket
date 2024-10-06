using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Queries.User.ViewModels;
public class UserProfileDto
{
    public string FistName { get; set; }
    public string LastName { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}