using System.ComponentModel.DataAnnotations;

using Airline.DTO;
using Airline.DTO.SeatDTOs;
using Airline.Exceptions;
using Airline.Services.Implementations;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("seat")]
public class SeatController(
    ISeatCreateService seatCreateService,
    SeatListService seatListService
) : ControllerBase
{
    private readonly ISeatCreateService _seatCreateService = seatCreateService;
    private readonly SeatListService _seatListService = seatListService;

    [HttpPost("create")]
    public async Task<IResult> Create([FromBody] SeatCreateRequestDTO createData)
    {
        try
        {
            await _seatCreateService.CreateAsync(createData);
            return Results.Created();
        }
        catch(EntityNotFoundException e)
        {
            return Results.NotFound(new { Message = e.Message });
        }
    }

    [HttpGet("list-available-for-ticket")]
    public async Task<IResult> ListAvailableForTicket([FromQuery] SeatListFilterDTO filters)
    {
        try
        {
            List<SeatTicketListDTO> seats = await _seatListService.ListAvailableSeatsForTicket(filters);
            return Results.Ok(seats);
        }
        catch(ValidationException e)
        {
            return Results.BadRequest(e.Message);
        }

    }
}