
using Airline.DTO.AirportDTOs;
using Airline.Exceptions;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("airport")]
public class AirportController(IAirportService airportService) : ControllerBase
{

    private readonly IAirportService _airportService = airportService;


    [HttpPost("create")]
    public async Task<IResult> CreateAirport(AirportCreateDTO data)
    {
        try
        {
            await _airportService.CreateAirport(data);

            return Results.Created();
        }
        catch(ValidationServiceException e)
        {
            return Results.BadRequest(e.Errors);
        }
        catch(Exception)
        {
            return Results.InternalServerError("Internal server error.");
        }
    }

    [HttpGet("list")]
    public async Task<IResult> ListAirports(AirportListFilters filters)
    {
        try
        {
            List<AirportListDetailDTO> airports = await _airportService.ListAirportsAsync(filters);
            return Results.Ok(airports);

        }
        catch(Exception)
        {
            return Results.InternalServerError("Internal server error.");
        }
    }
}