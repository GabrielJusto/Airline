
using Airline.Database;
using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Airline.Repositories.Implementations;

public class AircraftRepository(AirlineContext context) : IAircraftRepository
{
    private readonly AirlineContext _context = context;

    public Aircraft? GetAircraft(int aircraftId)
    {
        return _context.Aircrafts.FirstOrDefault(a => a.AircraftID == aircraftId);
    }

    public IReadOnlyList<Aircraft> ListAircrafts(AircraftListFiltersDTO filters)
    {
        return _context.Aircrafts
            .Skip((filters.Page - 1) * filters.PerPage)
            .Take(filters.PerPage)
            .ToList();
    }

    public void Insert(AircraftCreateDTO data)
    {
        Aircraft aircraft = new(data);
        _context.Aircrafts.Add(aircraft);
        _context.SaveChanges();
    }

    public void Update(AircraftUpdateDTO updateData)
    {
        Aircraft? aircraft = _context.Aircrafts.FirstOrDefault(a => a.AircraftID == updateData.AircraftId);

        if(aircraft == null)
        {
            throw new EntityNotFoundException("Aircraft not found");
        }

        if(updateData.Capacity.HasValue)
        {
            aircraft.Capacity = updateData.Capacity.Value;
        }

        if(updateData.Range.HasValue)
        {
            aircraft.Range = updateData.Range.Value;
        }

        if(updateData.AverageFuelConsumption != null)
        {
            aircraft.AverageFuelConsumption = updateData.AverageFuelConsumption.Value;
        }

        _context.SaveChanges();
    }

    public async Task DeleteAsync(int aircraftId)
    {
        Aircraft? aircraft = await _context.Aircrafts.FirstOrDefaultAsync(a => a.AircraftID == aircraftId);

        if(aircraft == null)
        {
            throw new EntityNotFoundException("Aircraft not found");
        }

        _context.Aircrafts.Remove(aircraft);
        await _context.SaveChangesAsync();
    }
}
