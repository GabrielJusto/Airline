using Airline.RequestBodies;

namespace Airline.Services.Interfaces;

public interface IRouteService
{
    public Task<int> CreateAsync(RouteInsertRequestBody data);
}