using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Basket.AddTicket;
using MyTicket.Application.Features.Commands.Basket.RemoveTicket;
using MyTicket.Application.Features.Queries.Basket;
using MyTicket.Infrastructure.BaseMessages;

namespace MyTicket.Controllers;
[Route("api/basket")]
[ApiController]
[Authorize]
public class BasketsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ITicketQueries _ticketQueries;

    public BasketsController(IMediator mediator, ITicketQueries ticketQueries)
    {
        _mediator = mediator;
        _ticketQueries = ticketQueries;
    }

    [HttpGet("get-basket")]
    public async Task<IActionResult> GetBasket()
    {
        return Ok(await _ticketQueries.GetTicketsInBasket());
    }

    [HttpPost("add-ticket")]
    public async Task<IActionResult> AddTicketToBasket([FromBody] AddTicketToBasketCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok(UIMessage.GetSuccessMessage)
             : BadRequest(UIMessage.GetFailureMessage);
    }

    [HttpDelete("remove-ticket")]
    public async Task<IActionResult> RemoveTicketFromBasket([FromBody] RemoveTicketFromBasketCommand command)
    {
        var result = await _mediator.Send(command);
        return result
            ? Ok(UIMessage.GetSuccessMessage)
            : BadRequest(UIMessage.GetFailureMessage);
    }
}

