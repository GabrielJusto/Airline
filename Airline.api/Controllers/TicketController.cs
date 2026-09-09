
using Airline.DTO;
using Airline.Services.Implementations;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("ticket")]
public class TicketController(
    TicketPurchaseService ticketPurchaseService
) : ControllerBase
{
    private readonly TicketPurchaseService _ticketPurchaseService = ticketPurchaseService;

    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseTicket([FromBody] TicketPurchaseRequestDTO request)
    {
        int ticketId = await _ticketPurchaseService.PurchaseTicketAsync(request);
        return Ok(new { TicketId = ticketId });
    }
}