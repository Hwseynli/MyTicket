﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTicket.Application.Features.Commands.Order.Create;
using MyTicket.Application.Features.Queries.OrderHistory;

namespace MyTicket.Controllers;
[Route("api/orders")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IOrderQueries _orderQueries;

    public OrderController(IMediator mediator, IOrderQueries orderQueries)
    {
        _mediator = mediator;
        _orderQueries = orderQueries;
    }

    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromForm] CreateOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("get-all-orders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderQueries.GetAllOrdersAsync();
        return Ok(orders);
    }

    [HttpGet("search-by-date")]
    public async Task<IActionResult> GetOrdersByDate([FromQuery] DateTime date)
    {
        var orders = await _orderQueries.GetOrdersByDateAsync(date);
        return Ok(orders);
    }

    [HttpGet("search-by-ticket/{ticketId}")]
    public async Task<IActionResult> GetOrdersByTicket(int ticketId)
    {
        var orders = await _orderQueries.GetOrdersByTicketAsync(ticketId);
        return Ok(orders);
    }

    [HttpGet("search-by-latest-order")]
    public async Task<IActionResult> GetLatestOrder()
    {
        var orders = await _orderQueries.GetLatestOrderAsync();
        return Ok(orders);
    }

    [HttpGet("search-by-latest-event")]
    public async Task<IActionResult> GetOrdersByLatestEvent()
    {
        var orders = await _orderQueries.GetOrdersByUpcomingEventAsync();
        return Ok(orders);
    }

    [HttpGet("download-receipt/{orderId}")]
    public async Task<IActionResult> DownloadReceipt(int orderId)
    {
        var pdfBytes = await _orderQueries.GetOrderReceiptAsync(orderId);
        return File(pdfBytes, "application/pdf", "OrderReceipt.pdf");
    }
}