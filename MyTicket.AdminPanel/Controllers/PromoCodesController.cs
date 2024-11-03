using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.PromoCode.Create;
using MyTicket.Application.Features.Commands.PromoCode.Delete;
using MyTicket.Application.Features.Commands.PromoCode.Update;

namespace MyTicket.AdminPanel.Controllers;
[Route("api/promoCodes")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PromoCodesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromoCodesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create-promoCode")]
    public async Task<IActionResult> CreatePromoCode([FromForm] CreatePromoCodeCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Succesfully") : BadRequest();
    }
    [HttpPut("update-promoCode")]
    public async Task<IActionResult> UpdatePromoCode([FromForm] UpdatePromoCodeCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Succesfully") : BadRequest();
    }
    [HttpDelete("delete-promoCode")]
    public async Task<IActionResult> DeletePromoCode([FromForm] DeletePromoCodeCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Succesfully") : BadRequest();
    }
}

