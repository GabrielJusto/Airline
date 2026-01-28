using System.ComponentModel.DataAnnotations;

using Airline.DTO;
using Airline.Models;
using Airline.Repositories.Interfaces;
using Airline.RequestBodies;
using Airline.Services.Interfaces;
using Airline.Validators;
using Airline.Validators.Route;

using Route = Airline.Models.Route;

namespace Airline.Services.Implementations;

public class CreateRouteService(
    IRouteRepository routeRepository
    ) : ICreateRouteService
{
    private readonly IRouteRepository _routeRepository = routeRepository;

    // public async Task<int> CreateAsync(RouteInsertRequestBody data)
    // {
    //   this.Validate(data);
    // RouteMergeDTO mergeData = new(data);

    // Route route = new(mergeData);

    //     return await _routeRepository.InsertAsync(route);
    // }

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