using System.ComponentModel.DataAnnotations;

using Airline.Exceptions;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.RequestBodies;
using Airline.Services.Interfaces;
using Airline.Validators;
using Airline.Validators.Route;

using Route = Airline.Models.Route;

namespace Airline.Services.Implementations;

public class RouteService(
    IRouteRepository routeRepository,
    IAirportService airportService
    ) : IRouteService
{
    private readonly IRouteRepository _routeRepository = routeRepository;
    private readonly IAirportService _airportService = airportService;

    public async Task<int> CreateAsync(RouteInsertRequestBody data)
    {
        Validate(data);

        Airport? fromAirport = await _airportService.GetAirportByIdAsync(data.FromAirportId) ?? throw new EntityNotFoundException("From airport not found.");
        Airport? toAirport = await _airportService.GetAirportByIdAsync(data.ToAirportId) ?? throw new EntityNotFoundException("Destination airport not found.");


        Route route = new(data)
        {
            FromAirport = fromAirport,
            ToAirport = toAirport
        };

        return await _routeRepository.InsertAsync(route);
    }

    private void Validate(RouteInsertRequestBody data)
    {
        IValidation[] validations =
        [
            new RouteAlreadyExistsValidation(_routeRepository, data)
        ];

        ValidatorService validator = new(validations);
        validator.Validate();

        if(validator.HasErrors())
        {
            throw new ValidationException(string.Join("; ", validator.GetErrors()));
        }
    }
}