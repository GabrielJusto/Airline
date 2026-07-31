
using Airline.DTO;

namespace Airline.Services.Interfaces;

public interface IAircraftService
{
    public bool CreateAircraft(AircraftCreateDTO createData);
    public IReadOnlyList<AircraftDetailDTO> ListAircrafts(AircraftListFiltersDTO filters);
    public AircraftDetailDTO? GetAircraftDetail(int aircraftId);
    public bool UpdateAircraft(AircraftUpdateDTO updateData);
    public Task<bool> DeleteAircraftAsync(int aircraftId);
}