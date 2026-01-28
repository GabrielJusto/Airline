using Airline.Database;
using Airline.DTO;
using Airline.Models;
using Airline.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Airline.Repositories.Implementations;

public class FlightRepository(AirlineContext context) : IFlightRepository
{
    private readonly AirlineContext _context = context;

    public async Task<int> Create(Flight flight)
    {
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
        return flight.FlightId;
    }

    public async Task<Flight?> GetByIdAsync(int id)
    {
        return await _context.Flights
            .Include(f => f.Aircraft)
            .Include(f => f.Route)
            .FirstOrDefaultAsync(f => f.FlightId == id);
    }

    public async Task<IEnumerable<Flight>> ListAsync(FlightListFilterDto filter)
    {

        IQueryable<Flight> query = _context.Flights
            .Include(f => f.Aircraft)
            .Include(f => f.Route);


        if(filter.StartDepartureDate.HasValue)
        {
            query = query.Where(f => f.Departure.Date >= filter.StartDepartureDate);
        }

        if(filter.EndDepartureDate.HasValue)
        {
            query = query.Where(f => f.Departure.Date <= filter.EndDepartureDate);
        }

        return await query.ToListAsync();
    }
}