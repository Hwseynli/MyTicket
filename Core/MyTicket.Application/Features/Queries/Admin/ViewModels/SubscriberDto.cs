using MyTicket.Application.Exceptions;

namespace MyTicket.Application.Features.Queries.Admin.ViewModels;
public class SubscriberDto
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime CreatedTime { get; set; }

    static SubscriberDto CreateDto(int id, string? phoneNumber, string? email, DateTime createdTime)
    {
        return new SubscriberDto
        {
            Id = id,
            PhoneNumber = phoneNumber,
            Email = email,
            CreatedTime = createdTime
        };
    }

    public static List<SubscriberDto> CreateDtos(IEnumerable<Domain.Entities.Users.Subscriber> subscribers)
    {
        if (subscribers is null)
            throw new NotFoundException();
        List<SubscriberDto> dtos = new List<SubscriberDto>();
        foreach (var item in subscribers)
        {
            var dto = CreateDto(
                id: item.Id,
                phoneNumber: item.PhoneNumber,
                email: item.Email,
                createdTime:item.CreatedDateTime
                );
            dtos.Add(dto);
        }
        return dtos;
    }
}