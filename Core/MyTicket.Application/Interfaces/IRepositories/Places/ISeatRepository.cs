using MyTicket.Domain.Entities.Places;

namespace MyTicket.Application.Interfaces.IRepositories.Places;
public interface ISeatRepository:IRepository<Seat>
{
    Task CreatSeatsAsync(int seatCount, int rowCount, int placeHallId, int userId, CancellationToken cancellationToken);
}

