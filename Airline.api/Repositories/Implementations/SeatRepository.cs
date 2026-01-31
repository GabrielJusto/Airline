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
        IQueryable<Seat> query = _context.Seats
            .Include(s => s.Flight)
            .ThenInclude(f => f.Route)
            .ThenInclude(r => r.FromAirport)
            .Include(s => s.Flight)
            .ThenInclude(f => f.Route)
            .ThenInclude(r => r.ToAirport)
            .AsQueryable();

        if(filter.FlightId.HasValue)
        {
            query = query.Where(s => s.Flight.FlightId == filter.FlightId);
        }
        if(filter.RouteId.HasValue)
        {
            query = query.Where(s => s.Flight.RouteId == filter.RouteId);
        }
        if(filter.DepartureDate.HasValue)
        {
            DateTimeOffset start = filter.DepartureDate.Value.ToUniversalTime();
            DateTimeOffset end = start.AddDays(1);
            query = query.Where(s => s.Flight.Departure >= start && s.Flight.Departure < end);
        }

        return await Task.FromResult(query.ToList());
    }

    public async Task<IEnumerable<Seat>> ListForTicketAsync(SeatListFilterDTO filter)
    {
        IQueryable<Seat> query = _context.Seats.AsQueryable();

        if(filter.FlightId.HasValue)
        {
            query = query.Where(s => s.Flight.FlightId == filter.FlightId);
        }
        if(filter.RouteId.HasValue)
        {
            query = query.Where(s => s.Flight.RouteId == filter.RouteId);
        }

        return await Task.FromResult(query.ToList());
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