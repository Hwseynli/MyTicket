using MyTicket.Application.Exceptions;
using MyTicket.Domain.Entities.Enums;
using MyTicket.Domain.Entities.Places;

namespace MyTicket.Application.Features.Queries.Location.ViewModels;
public class SeatDto
{
    public int Id { get; set; }
    public int RowNumber { get; private set; }
    public int SeatNumber { get; private set; }
    public SeatType SeatType { get; set; }
    public decimal Price { get; private set; }
    public string PlaceHallName { get; set; }
    public int? UpdateById { get; protected set; }
    public DateTime? LastUpdateDateTime { get; protected set; }
    public int CreatedById { get; protected set; }
    public DateTime RecordDateTime { get; protected set; }

    static SeatDto CreateDto(int id, int rowNumber, int seatNumber,SeatType seatType,decimal price, string placeHallName, int createdById, DateTime createdTime, int? updatedById, DateTime? UpdatedTime)
    {
        return new SeatDto
        {
            Id = id,
            RowNumber = rowNumber,
            SeatNumber=seatNumber,
            SeatType=seatType,
            Price=price,
            PlaceHallName = placeHallName,
            UpdateById = updatedById,
            LastUpdateDateTime = UpdatedTime,
            CreatedById = createdById,
            RecordDateTime = createdTime
        };
    }

    public static List<SeatDto> CreateDtos(IEnumerable<Seat> seats)
    {
        if (seats is null)
            throw new NotFoundException();
        List<SeatDto> dtos = new List<SeatDto>();
        foreach (var item in seats)
        {
            var dto = CreateDto(
                id: item.Id,
               seatNumber: item.SeatNumber,
               rowNumber: item.RowNumber,
               seatType:item.SeatType,
               placeHallName: item.PlaceHall.Name,
               price:item.Price,
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

