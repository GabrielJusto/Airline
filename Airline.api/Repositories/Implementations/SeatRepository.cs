using Airline.Database;
using Airline.DTO;
using Airline.Models;
using Airline.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Airline.Repositories.Implementations;

public class SeatRepository(AirlineContext context) : ISeatRepository
{
    private readonly AirlineContext _context = context;

    public async Task AddRangeAsync(IEnumerable<Seat> seats)
    {
        _context.Seats.AddRange(seats);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Seat>> ListAsync(SeatListFilterDTO filter)
    {
        IQueryable<Seat> query = _context.Seats.AsQueryable();

        if(!string.IsNullOrWhiteSpace(filter.FromIATACode))
        {
            query = query.Where(s => s.Flight.Route.FromAirport.IATACode == filter.FromIATACode);
        }
        if(!string.IsNullOrWhiteSpace(filter.ToIATACode))
        {
            query = query.Where(s => s.Flight.Route.ToAirport.IATACode == filter.ToIATACode);
        }
        if(filter.FlightId.HasValue)
        {
            query = query.Where(s => s.Flight.FlightId == filter.FlightId);
        }
        if(filter.DepartureDate.HasValue)
        {
            DateTimeOffset start = filter.DepartureDate.Value.ToUniversalTime();
            DateTimeOffset end = start.AddDays(1);
            query = query.Where(s => s.Flight.Departure >= start && s.Flight.Departure < end);
        }

        return await query
            .Include(s => s.Flight)
                .ThenInclude(f => f.Route)
                    .ThenInclude(r => r.FromAirport)
            .Include(s => s.Flight)
                .ThenInclude(f => f.Route)
                    .ThenInclude(r => r.ToAirport)
            .AsNoTracking()
            .ToListAsync();

    }

    public async Task<Seat?> GetSeatByIdAsync(int seatId)
    {
        return await _context.Seats.FindAsync(seatId);
    }

    public async Task UpdateAsync(Seat seat)
    {
        _context.Seats.Update(seat);
        await _context.SaveChangesAsync();
    }

}