using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
using MyTicket.Application.Features.Commands.WishList.Add;
using MyTicket.Application.Features.Commands.WishList.Remove;
using MyTicket.Application.Features.Queries.Event;

namespace MyTicket.Controllers;
[Route("api/favourites")]
[ApiController]
[Authorize]
public class FavouritesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IEventQueries _queries;

    public FavouritesController(IMediator mediator, IEventQueries queries)
    {
        _mediator = mediator;
        _queries = queries;
    }

    // POST api/values
    [HttpPost("rate-events")]
    public async Task<IActionResult> RateEvent([FromBody] RateEventCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : BadRequest();
    }

    // GET: api/values
    [HttpGet("get-wishList")]
    public async Task<IActionResult> GetWishList()
    {
        var result = await _queries.GetWishList();
        return Ok(result);
    }

    // POST api/values
    [HttpPost("add - wishLsit")]
    public async Task<IActionResult> AddWishLists([FromBody] AddWishListCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("remove - wishList")]
    public async Task<IActionResult> RemoveFromWishList([FromBody] RemoveFromWishListCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

