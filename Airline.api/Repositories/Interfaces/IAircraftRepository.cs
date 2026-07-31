
using Airline.DTO;
using Airline.Models;

namespace Airline.Repositories.Interfaces;

public interface IAircraftRepository
{
    public IReadOnlyList<Aircraft> ListAircrafts(AircraftListFiltersDTO filters);
    public Aircraft? GetAircraft(int aircraftId);
    public void Insert(AircraftCreateDTO createData);
    public void Update(Aircraft aircraft);
    public Task DeleteAsync(int aircraftId);
}
