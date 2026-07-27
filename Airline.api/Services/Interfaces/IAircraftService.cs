
using Airline.DTO;

namespace Airline.Services.Interfaces;

public interface IAircraftService
{
    public bool CreateAircraft(AircraftCreateDTO createData);
    public IReadOnlyList<AircraftDetailDTO> ListAircrafts(AircraftListFiltersDTO filters);
}