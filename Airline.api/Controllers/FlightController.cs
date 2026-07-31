using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;


[ApiController]
[Route("flight")]
public class FlightController(
    IFlightService flightService,
    IFlightRepository flightRepository
    ) : ControllerBase
{

    private readonly IFlightService _flightService = flightService;
    private readonly IFlightRepository _flightRepository = flightRepository;

    [HttpPost("create")]
    public async Task<IResult> Create([FromBody] FlightCreateDTO data)
    {
        try
        {
            int flightId = await _flightService.Create(data);
            return Results.Created();
        }
        catch(EntityNotFoundException ex)
        {
            return Results.NotFound(new { Message = ex.Message });
        }
    }

    [HttpGet("{flightId}")]
    public async Task<IResult> Detail([FromRoute] int flightId)
    {
        try
        {
            FlightDetailDTO flight = await _flightService.Detail(flightId);
            return Results.Ok(flight);
        }
        catch(EntityNotFoundException ex)
        {
            return Results.NotFound(new { Message = ex.Message });
        }
    }

    [HttpGet("list")]
    public async Task<IResult> List([FromQuery] FlightListFilterDto filter)
    {
        IEnumerable<Flight> flights = await _flightRepository.ListAsync(filter);
        IEnumerable<FlightListDTO> flightDtos = flights.Select(flight => new FlightListDTO(flight));
        return Results.Ok(flightDtos);
    }
}