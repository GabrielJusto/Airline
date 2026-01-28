using Airline.DTO.AirportDTOs;
using Airline.Models;

namespace Airline.Services.Interfaces;

public interface IAirportService
{
    Task<int> CreateAirport(AirportCreateDTO data);
    Task<List<AirportListDetailDTO>> ListAirportsAsync(AirportListFilters filters);
    Task<Airport?> GetAirportByIdAsync(int airportId);
}
