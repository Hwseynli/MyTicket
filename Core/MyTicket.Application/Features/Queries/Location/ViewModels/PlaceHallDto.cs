using MyTicket.Application.Exceptions;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Application.Features.Queries.Location.ViewModels;
public class PlaceHallDto
{
    public int Id { get; set; }
    public string Name { get; private set; }
    public string PlaceName { get; set; }
    public int SeatCount { get; set; }
    public int? UpdateById { get; protected set; }
    public DateTime? LastUpdateDateTime { get; protected set; }
    public int CreatedById { get; protected set; }
    public DateTime RecordDateTime { get; protected set; }

    static PlaceHallDto CreateDto(int id, string name, string placeName, int seatCount, int createdById, DateTime createdTime, int? updatedById, DateTime? UpdatedTime)
    {
        return new PlaceHallDto
        {
            Id = id,
            Name=name,
            PlaceName=placeName,
            SeatCount=seatCount,
            UpdateById=updatedById,
            LastUpdateDateTime=UpdatedTime,
            CreatedById=createdById,
            RecordDateTime=createdTime
        };
    }

    public static List<PlaceHallDto> CreateDtos(IEnumerable<PlaceHall> placeHalls)
    {
        if (placeHalls is null)
            throw new NotFoundException();
        List<PlaceHallDto> dtos = new List<PlaceHallDto>();
        foreach (var item in placeHalls)
        {
            var dto = CreateDto(
                id: item.Id,
               name:item.Name,
               placeName:item.Place.Name,
               seatCount:item.Seats.Capacity,
               createdById:item.CreatedById,
               createdTime:item.RecordDateTime,
               updatedById:item.UpdateById,
               UpdatedTime:item.LastUpdateDateTime
                );
            dtos.Add(dto);
        }
        return dtos;
    }
}