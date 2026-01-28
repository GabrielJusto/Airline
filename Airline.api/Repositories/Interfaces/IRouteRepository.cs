using Airline.DTO;

using Route = Airline.Models.Route;

namespace Airline.Repositories.Interfaces;

public interface IRouteRepository
{
    public Task<int> InsertAsync(Route route);
    public Task<List<RouteListDTO>> ListAsync(RouteListFiltersDTO filters);
    public List<RouteListDTO> List(RouteListFiltersDTO filters);
    public Task<Route?> GetByIdAsync(int id);
}
