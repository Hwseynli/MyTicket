using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Basket.AddTicket;
using MyTicket.Application.Features.Commands.Basket.RemoveTicket;
using MyTicket.Application.Features.Queries.Basket;

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

    // GET: api/values
    [HttpGet("Tickets")]
    public async Task<IActionResult> GetTickets()
    {
        return Ok(await _ticketQueries.GetTicketsInBasket());
    }

    // POST api/Basket/AddTicket
    [HttpPost("AddTicket")]
    public async Task<IActionResult> AddTicketToBasket([FromBody] AddTicketToBasketCommand command)
    {
        // Tiketi əlavə et
        var result = await _mediator.Send(command);
        if (result)
        {
            return Ok("Ticket basketə əlavə olundu.");
        }
        return BadRequest("Ticket basketə əlavə olunmadı.");
    }

    // DELETE api/Basket/RemoveTicket
    [HttpDelete("RemoveTicket")]
    public async Task<IActionResult> RemoveTicketFromBasket([FromBody] RemoveTicketFromBasketCommand command)
    {
        // Tiketi sil
        var result = await _mediator.Send(command);
        if (result)
        {
            return Ok("Ticket basketdən silindi.");
        }
        return BadRequest("Ticket basketdən silinmədi.");
    }
}