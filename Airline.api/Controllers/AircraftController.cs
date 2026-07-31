using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.RequestBodies;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace Airline.Controllers;

[ApiController]
[Route("aircraft")]
public class AircraftController(
    IAircraftRepository aircraftRepository,
    IAircraftService aircraftService
) : ControllerBase
{

    private readonly IAircraftRepository _aircraftRepository = aircraftRepository;
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
        try
        {
            AircraftDetailDTO? aircraftDetail = _aircraftService.GetAircraftDetail(aircraftId);
            return Results.Ok(aircraftDetail);
        }
        catch(EntityNotFoundException e)
        {
            return Results.NotFound(new { Message = e.Message });
        }
        catch(Exception)
        {
            return Results.InternalServerError(new { Message = "An error occurred while retrieving the aircraft details." });
        }
    }

    [HttpPatch("update/{id}")]
    public IResult Update([FromBody] AircraftUpdateRequestBody updateData, int id)
    {
        try
        {
            AircraftUpdateDTO updateDto = new(updateData, id);
            _aircraftService.UpdateAircraft(updateDto);
            return Results.NoContent();
        }
        catch(EntityNotFoundException e)
        {
            return Results.NotFound(new { Message = e.Message });
        }catch(Exception)
        {
            return Results.InternalServerError(new { Message = "An error occurred while updating the aircraft." });
        }
    }

    [HttpDelete("{aircraftId}")]
    public async Task<IResult> RemoveAsync(int aircraftId)
    {
        try
        {
            await _aircraftRepository.DeleteAsync(aircraftId);
            return Results.Ok();
        }
        catch(EntityNotFoundException e)
        {
            return Results.NotFound(new { Message = e.Message });
        }
    }
}