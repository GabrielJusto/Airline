
using Airline.DTO;
using Airline.Repositories.Interfaces;
using Airline.RequestBodies;
using Airline.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;

using Route = Airline.Models.Route;

namespace Airline.Controllers;

[ApiController]
[Route("route")]
public class RouteController(IRouteRepository routeRepository) : ControllerBase
{

    private readonly IRouteRepository _routeRepository = routeRepository;
    [HttpPost("create")]
    public async Task<IResult> Create(
        [FromBody] RouteInsertRequestBody createData,
        [FromServices] IRouteService createRouteService
    )
    {
        await createRouteService.CreateAsync(createData);
        return Results.Created();
    }


    [HttpGet("list")]
    public async Task<IResult> List([FromQuery] RouteListFiltersDTO filters)
    {
        IEnumerable<RouteListDTO> routes = await _routeRepository.ListAsync(filters);
        return Results.Ok(routes);
    }
}


