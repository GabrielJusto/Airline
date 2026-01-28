using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Airline.DTO;
using Airline.Repositories.Interfaces;
using Airline.RequestBodies;

namespace Airline.Validators.Route;

public class RouteAlreadyExistsValidation(
    IRouteRepository routeRepository,
    RouteInsertRequestBody data
) : IValidation
{
    private readonly IRouteRepository _routeRepository = routeRepository;
    private readonly RouteInsertRequestBody _data = data;


    public void Validate()
    {
        RouteListFiltersDTO filters = new()
        {
            FromAirportId = _data.FromAirportId,
            ToAirportId = _data.ToAirportId
        };

        List<RouteListDTO> routes = _routeRepository.List(filters);

        if(routes.Count != 0)
        {
            throw new ValidationException("Route already exists.");
        }
    }
}