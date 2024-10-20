using MyTicket.Application.Exceptions;
using MyTicket.Domain.Entities.Enums;

namespace MyTicket.Application.Features.Queries.Admin.ViewModels;
public class SubscriberDto
{
    public int Id { get; set; }
    public string? EmailOrPhoneNumber { get; set; }
    public StringType? StringType { get; set; }
    public DateTime CreatedTime { get; set; }

    static SubscriberDto CreateDto(int id, StringType? stringType, string? emailOrPhoneNumber, DateTime createdTime)
    {
        return new SubscriberDto
        {
            Id = id,
            StringType = stringType,
            EmailOrPhoneNumber = emailOrPhoneNumber,
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
                stringType: item.StringType,
                emailOrPhoneNumber: item.EmailOrPhoneNumber,
                createdTime:item.CreatedDateTime
                );
            dtos.Add(dto);
        }
        return dtos;
    }
}