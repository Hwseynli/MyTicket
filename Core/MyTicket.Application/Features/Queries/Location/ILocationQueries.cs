using MyTicket.Application.Features.Queries.Location.ViewModels;

namespace MyTicket.Application.Features.Queries.Location;
public interface ILocationQueries
{
    Task<List<PlaceDto>> GetPlaces();
    Task<List<PlaceHallDto>> GetPlaceHalls();
    Task<List<SeatDto>> GetSeats();
}

