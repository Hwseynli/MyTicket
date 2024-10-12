using System;
using MyTicket.Application.Interfaces.IRepositories.Places;
using MyTicket.Domain.Entities.Places;
using MyTicket.Persistence.Context;

namespace MyTicket.Persistence.Repositories.Places;
public class PlaceHallRepository : Repository<PlaceHall>, IPlaceHallRepository
{
    public PlaceHallRepository(AppDbContext context) : base(context)
    {
    }
}

