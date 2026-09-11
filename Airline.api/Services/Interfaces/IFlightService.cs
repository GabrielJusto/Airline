using Airline.DTO;
using Airline.Models;

namespace Airline.Services.Interfaces;

public interface IFlightService
{
    public Task<int> Create(FlightCreateDTO data);
    public Task<FlightDetailDTO> Detail(int flightId);
    public Task<Flight?> GetByIdAsync(int flightId);
}