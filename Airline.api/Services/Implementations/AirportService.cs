

using Airline.DTO.AirportDTOs;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.Validators;
using Airline.Validators.Airport;

namespace Airline.Services.Implementations;

public class AirportService(IAirportRepository airportRepository)
{
    private readonly IAirportRepository _airportRepository = airportRepository;

    public async Task<int> CreateAirport(AirportCreateDTO data)
    {
        ValidatorService validator = new([new IATACodeValidation(data)]);
        validator.ValidateAndThrow();

        Airport airport = new(data);

        return await _airportRepository.InsertAirportAsync(airport);
    }

    public async Task<List<AirportListDetailDTO>> ListAirportsAsync(AirportListFilters filters)
    {
        List<Airport> airports = await _airportRepository.ListAirportsAsync(filters);
        return airports.Select(a => new AirportListDetailDTO(a)).ToList();
    }
}