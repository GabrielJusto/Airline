using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Services.Interfaces;

using Route = Airline.Models.Route;

namespace Airline.Services.Implementations;

public class FlightService(
    IFlightRepository flightRepository,
    IAircraftRepository aircraftRepository,
    IRouteRepository routeRepository
) : IFlightService
{
    private readonly IFlightRepository _flightRepository = flightRepository;
    private readonly IAircraftRepository _aircraftRepository = aircraftRepository;
    private readonly IRouteRepository _routeRepository = routeRepository;

    public async Task<int> Create(FlightCreateDTO data)
    {
        Aircraft? aircraft = _aircraftRepository.GetAircraft(data.AircraftId);
        if(aircraft == null)
            throw new EntityNotFoundException("Aircraft not found");

        Route? route = await _routeRepository.GetByIdAsync(data.RouteId);
        if(route == null)
            throw new EntityNotFoundException("Route not found");

        Flight flight = new(data)
        {
            Aircraft = aircraft,
            Route = route
        };

        return await _flightRepository.Create(flight);
    }

    public async Task<FlightDetailDTO> Detail(int flightId)
    {
        Flight? flight = await _flightRepository.GetByIdAsync(flightId);
        if(flight == null)
        {
            throw new EntityNotFoundException("Flight not found");
        }

        return new FlightDetailDTO(flight);
    }
}