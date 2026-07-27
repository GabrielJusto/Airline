using Airline.DTO;
using Airline.Repositories.Interfaces;
using Airline.Services.Interfaces;

namespace Airline.Services.Implementations;

public class AircraftService(
    IAircraftRepository aircraftRepository
) : IAircraftService
{
    private readonly IAircraftRepository _aircraftRepository = aircraftRepository;
    
    public bool CreateAircraft(AircraftCreateDTO createData)
    {
        try
        {
            _aircraftRepository.Insert(createData);
            return true;
        }catch(Exception)
        {
            return false;
        }
        
    }

    public IReadOnlyList<AircraftDetailDTO> ListAircrafts(AircraftListFiltersDTO filters)
    {
        try
        {
            return _aircraftRepository.ListAircrafts(filters)
                .Select(a => new AircraftDetailDTO(a))
                .ToList();

        }catch(Exception)
        {
            throw;
        }
    }
}