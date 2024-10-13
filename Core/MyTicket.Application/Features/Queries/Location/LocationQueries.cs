using MyTicket.Application.Features.Queries.Location.ViewModels;
using MyTicket.Application.Interfaces.IRepositories.Places;

namespace MyTicket.Application.Features.Queries.Location;
public class LocationQueries : ILocationQueries
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IPlaceHallRepository _placeHallRepository;
    private readonly ISeatRepository _seatRepository;

    public LocationQueries(IPlaceRepository placeRepository, IPlaceHallRepository placeHallRepository, ISeatRepository seatRepository)
    {
        _placeRepository = placeRepository;
        _placeHallRepository = placeHallRepository;
        _seatRepository = seatRepository;
    }

    public async Task<List<PlaceDto>> GetPlaces()
    {
        var places = await _placeRepository.GetAllAsync(includes:"PlaceHall");
        return PlaceDto.CreateDtos(places);
    }

    public async Task<List<SeatDto>> GetSeats()
    {
        var seats = await _seatRepository.GetAllAsync(includes:"PlaceHall");
        return SeatDto.CreateDtos(seats);
    }

    public async Task<List<PlaceHallDto>> GetPlaceHalls()
    {
        var halls = await _placeHallRepository.GetAllAsync(includes:"Seat,Place");
        return PlaceHallDto.CreateDtos(halls);
    }
}