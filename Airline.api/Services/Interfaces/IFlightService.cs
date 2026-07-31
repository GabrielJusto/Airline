using Airline.DTO;

namespace Airline.Services.Interfaces;

public interface IFlightService
{
    public Task<int> Create(FlightCreateDTO data);
    public Task<FlightDetailDTO> Detail(int flightId);
}