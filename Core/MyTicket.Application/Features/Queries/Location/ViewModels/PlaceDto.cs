using MyTicket.Application.Exceptions;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Application.Features.Queries.Location.ViewModels;
public class PlaceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public int PlaceHallCount { get; set; }
    public int? UpdateById { get; protected set; }
    public DateTime? LastUpdateDateTime { get; protected set; }
    public int CreatedById { get; protected set; }
    public DateTime RecordDateTime { get; protected set; }

    static PlaceDto CreateDto(int id, string name, string address, int placeHallCount, int createdById, DateTime createdTime, int? updatedById, DateTime? UpdatedTime)
    {
        return new PlaceDto
        {
            Id = id,
            Name = name,
            PlaceHallCount=placeHallCount,
            Address=address,
            UpdateById = updatedById,
            LastUpdateDateTime = UpdatedTime,
            CreatedById = createdById,
            RecordDateTime = createdTime
        };
    }

    public static List<PlaceDto> CreateDtos(IEnumerable<Place> places)
    {
        if (places is null)
            throw new NotFoundException();
        List<PlaceDto> dtos = new List<PlaceDto>();
        foreach (var item in places)
        {
            var dto = CreateDto(
                id: item.Id,
               name: item.Name,
               address:item.Address,
               placeHallCount: item.PlaceHalls.Capacity,
               createdById: item.CreatedById,
               createdTime: item.RecordDateTime,
               updatedById: item.UpdateById,
               UpdatedTime: item.LastUpdateDateTime
                );
            dtos.Add(dto);
        }
        return dtos;
    }
}

