﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Admin.Rating.RateEvent;
using MyTicket.Application.Features.Commands.WishList.Add;
using MyTicket.Application.Features.Commands.WishList.Remove;
using MyTicket.Application.Features.Queries.Favourites;

namespace MyTicket.Controllers;
[Route("api/favourites")]
[ApiController]
[Authorize]
public class FavouritesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFavouriteQueries _queries;

    public FavouritesController(IMediator mediator, IFavouriteQueries queries)
    {
        _mediator = mediator;
        _queries = queries;
    }

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
    [HttpPost("add-wishlist-event")]
    public async Task<IActionResult> AddWishLists([FromBody] AddWishListCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("remove-wishlist-event")]
    public async Task<IActionResult> RemoveFromWishList([FromBody] RemoveFromWishListCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : BadRequest();
    }
}
