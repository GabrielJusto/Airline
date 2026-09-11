using Airline.DTO;
using Airline.DTO.SeatDTOs;
using Airline.Services.Implementations;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("seat")]
public class SeatController(
    ISeatCreateService seatCreateService,
    ISeatService seatService
) : ControllerBase
{
    private readonly ISeatCreateService _seatCreateService = seatCreateService;
    private readonly ISeatService _seatService = seatService;

    [HttpPost("create")]
    public async Task<IResult> Create([FromBody] SeatCreateRequestDTO createData)
    {
        await _seatCreateService.CreateAsync(createData);
        return Results.Created();
    }

    [HttpGet("list-available-for-ticket")]
    public async Task<IResult> ListAvailableForTicket([FromQuery] SeatListFilterDTO filters)
    {
        IReadOnlyList<SeatTicketListDTO> seats = await _seatService.ListAvailableSeatsForTicketAsync(filters);
        return Results.Ok(seats);
    }
}