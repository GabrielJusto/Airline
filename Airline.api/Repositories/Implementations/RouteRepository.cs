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

    public async Task<List<RouteListDTO>> ListAsync(RouteListFiltersDTO filters)
    {
        IQueryable<Route> query = _context.Routes.AsQueryable();

        if(filters.FromAirportId != null)
        {
            query = query.Where(r => r.FromAirportId == filters.FromAirportId);
        }
        if(filters.ToAirportId != null)
        {
            query = query.Where(r => r.ToAirportId == filters.ToAirportId);
        }

        List<RouteListDTO> routes = await query
            .Select(r => new RouteListDTO(
                r.RouteID
            )).ToListAsync();

        return routes;
    }

    public List<RouteListDTO> List(RouteListFiltersDTO filters)
    {
        IQueryable<Route> query = _context.Routes.AsQueryable();

        if(filters.FromAirportId != null)
        {
            query = query.Where(r => r.FromAirportId == filters.FromAirportId);
        }
        if(filters.ToAirportId != null)
        {
            query = query.Where(r => r.ToAirportId == filters.ToAirportId);
        }

        List<RouteListDTO> routes = query
            .Select(r => new RouteListDTO(
                r.RouteID
            )).ToList();

        return routes;
    }
    public async Task<Route?> GetByIdAsync(int id)
    {
        return await _context.Routes.FirstOrDefaultAsync(r => r.RouteID == id);
    }
}
