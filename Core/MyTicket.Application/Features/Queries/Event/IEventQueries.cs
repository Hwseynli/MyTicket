namespace MyTicket.Application.Features.Queries.Event;
public interface IEventQueries
{
    Task<double> GetRating(int eventId);
}

