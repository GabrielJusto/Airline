using Airline.DTO;
using Airline.Models;
using Airline.RequestBodies;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("aircraft")]
public class AircraftController(
    IAircraftService aircraftService
) : ControllerBase
{
    private readonly IAircraftService _aircraftService = aircraftService;

    [HttpPost("create")]
    public IResult Create([FromBody] AircraftCreateDTO createData)
    {

        bool created = _aircraftService.CreateAircraft(createData);

        if(created)
        {
            return Results.Created();
        }
        else
        {
            return Results.InternalServerError(new { Message = "Failed to create aircraft." });
        }

    }

    [HttpGet("list")]
    public IResult List(
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10)
    {
        AircraftListFiltersDTO filters = new()
        {
            Page = page,
            PerPage = perPage
        };
        return Results.Ok(_aircraftService.ListAircrafts(filters));
    }


    [HttpGet("{aircraftId}")]
    public IResult Detail(int aircraftId)
    {
        AircraftDetailDTO? aircraftDetail = _aircraftService.GetAircraftDetail(aircraftId);
        return Results.Ok(aircraftDetail);

    }

    [HttpPatch("update/{id}")]
    public IResult Update([FromBody] AircraftUpdateRequestBody updateData, int id)
    {
        AircraftUpdateDTO updateDto = new(updateData, id);
        _aircraftService.UpdateAircraft(updateDto);
        return Results.NoContent();

    }

    [HttpDelete("{aircraftId}")]
    public async Task<IResult> RemoveAsync(int aircraftId)
    {
        if(await _aircraftService.DeleteAircraftAsync(aircraftId))
        {
            return Results.NoContent();
        }
        return Results.InternalServerError(new { Message = "Failed to delete aircraft." });
    }
}