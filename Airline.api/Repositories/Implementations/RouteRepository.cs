using Airline.Database;
using Airline.DTO;
using Airline.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using Route = Airline.Models.Route;



namespace Airline.Repositories.Implementations;

public class RouteRepository(AirlineContext context) : IRouteRepository
{
    private readonly AirlineContext _context = context;
    public async Task<int> InsertAsync(Route route)
    {
        await _context.Routes.AddAsync(route);
        await _context.SaveChangesAsync();
        return route.RouteID;
    }

    public async Task<IEnumerable<RouteListDTO>> ListAsync(RouteListFiltersDTO filters)
    {
        IQueryable<Route> query = _context.Routes.AsQueryable();

        List<RouteListDTO> routes = await query
            .Select(r => new RouteListDTO(
                r.RouteID
            )).ToListAsync();

        return routes;
    }
    public async Task<Route?> GetByIdAsync(int id)
    {
        return await _context.Routes.FirstOrDefaultAsync(r => r.RouteID == id);
    }
}
