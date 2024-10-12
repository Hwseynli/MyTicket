using System;
using MediatR;

namespace MyTicket.Application.Features.Commands.Place.Hall.Create
{
    public class AddPlaceHallCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public int PlaceId { get; set; }
    }
}

