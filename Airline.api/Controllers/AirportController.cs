
using Airline.DTO.AirportDTOs;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("airport")]
public class AirportController(IAirportService airportService) : ControllerBase
{

    private readonly IAirportService _airportService = airportService;


    [HttpPost("create")]
    public async Task<IResult> CreateAirport([FromBody] AirportCreateDTO data)
    {

        await _airportService.CreateAirport(data);
        return Results.Created();

    }

    [HttpGet("list")]
    public async Task<IResult> ListAirports([FromQuery] AirportListFilters filters)
    {
        List<AirportListDetailDTO> airports = await _airportService.ListAirportsAsync(filters);
        return Results.Ok(airports);
    }
}