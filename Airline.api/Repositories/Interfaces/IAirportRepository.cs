using Airline.DTO;
using Airline.DTO.AirportDTOs;
using Airline.Models;

namespace Airline.Repositories.Interfaces;

public interface IAirportRepository
{
    public Task<int> InsertAirportAsync(Airport airport);
    public Task<List<Airport>> ListAirportsAsync(AirportListFilters filters);
    public Task<Airport?> GetAirportById(int airportId);
}
