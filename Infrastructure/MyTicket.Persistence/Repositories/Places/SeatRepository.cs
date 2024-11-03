using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Places;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Places;
public class SeatRepository : Repository<Seat>, ISeatRepository
{
    public SeatRepository(AppDbContext context) : base(context)
    {
    }

    public async Task CreatSeatsAsync(int seatCount, int rowCount, int placeHallId, int userId, CancellationToken cancellationToken)
    {
        int seatsPerRow = seatCount / rowCount;
        for (int row = 1; row <= rowCount; row++)
        {
            for (int seat = 1; seat <= seatsPerRow; seat++)
            {
                // Yeni oturacaq əlavə edirik
                var newSeat = new Seat();
                var seatType = newSeat.DetermineSeatType(row, rowCount);
                newSeat.SetDetail(row, seat, seatType, newSeat.CalculateSeatPrice(seatType), placeHallId, userId);
                await AddAsync(newSeat);
                await Commit(cancellationToken);
            }
        }
    }
}

