using Airline.Database;
using Airline.DTO.AirportDTOs;
using Airline.Models;
using Airline.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Airline.Repositories.Implementations;

public class AirportRepository(AirlineContext context) : IAirportRepository
{
    private readonly AirlineContext _context = context;

    public async Task<int> InsertAirportAsync(Airport airport)
    {
        await _context.AddAsync(airport);
        await _context.SaveChangesAsync();

        return airport.AirportId;
    }

    public async Task<List<Airport>> ListAirportsAsync(AirportListFilters filters)
    {
        IQueryable<Airport> query = _context.Airports;

        if(filters.City != null)
        {
            query = query.Where(a => a.City.Equals(filters.City));
        }
        if(filters.Country != null)
        {
            query = query.Where(a => a.Country.Equals(filters.Country));
        }

        return await query.ToListAsync();
    }

    public async Task<Airport?> GetAirportById(int airportId)
    {
        return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportId);
    }

}