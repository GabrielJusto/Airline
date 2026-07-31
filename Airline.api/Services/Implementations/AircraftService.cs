using System.ComponentModel.DataAnnotations;

using Airline.DTO;
using Airline.Exceptions;
using Airline.Models;
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
        ValidateCreateData(createData);
        try
        {
            _aircraftRepository.Insert(createData);
            return true;
        }
        catch(Exception)
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

        }
        catch(Exception)
        {
            throw;
        }
    }

    public AircraftDetailDTO? GetAircraftDetail(int aircraftId)
    {
        try
        {
            Aircraft? aircraft = _aircraftRepository.GetAircraft(aircraftId);
            if(aircraft == null)
            {
                throw new EntityNotFoundException($"Aircraft with ID {aircraftId} not found.");
            }
            return new AircraftDetailDTO(aircraft);
        }
        catch(Exception)
        {
            throw;
        }
    }

    public bool UpdateAircraft(AircraftUpdateDTO updateData)
    {
        ValidateUpdateData(updateData);

        try
        {
            Aircraft? aircraft = _aircraftRepository.GetAircraft(updateData.AircraftId);

            if(aircraft == null)
            {
                throw new EntityNotFoundException("Aircraft not found");
            }

            if(updateData.Capacity.HasValue)
            {
                aircraft.Capacity = updateData.Capacity.Value;
            }

            if(updateData.Range.HasValue)
            {
                aircraft.Range = updateData.Range.Value;
            }

            if(updateData.AverageFuelConsumption != null)
            {
                aircraft.AverageFuelConsumption = updateData.AverageFuelConsumption.Value;
            }

            _aircraftRepository.Update(aircraft);

            return true;
        }
        catch(EntityNotFoundException)
        {
            throw;
        }
        catch(Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAircraftAsync(int aircraftId)
    {
        try
        {
            Aircraft? aircraft = _aircraftRepository.GetAircraft(aircraftId);

            if(aircraft == null)
            {
                throw new EntityNotFoundException("Aircraft not found");
            }

            await _aircraftRepository.DeleteAsync(aircraftId);

            return true;
        }
        catch(EntityNotFoundException)
        {
            throw;
        }
        catch(Exception)
        {
            return false;
        }
    }

    private static void ValidateUpdateData(AircraftUpdateDTO updateData)
    {
        ValidateNonNegativeValues(updateData.Capacity, updateData.Range, updateData.AverageFuelConsumption);
    }

    private static void ValidateCreateData(AircraftCreateDTO createData)
    {
        ValidateNonNegativeValues(createData.Capacity, createData.Range, createData.AverageFuelConsumption);
    }

    private static void ValidateNonNegativeValues(
        int? capacity,
        double? range,
        double? averageFuelConsumption)
    {
        List<string> errors = [];

        if(capacity.HasValue && capacity.Value < 0)
        {
            errors.Add("Capacity cannot be negative.");
        }

        if(range.HasValue && range.Value < 0)
        {
            errors.Add("Range cannot be negative.");
        }

        if(averageFuelConsumption.HasValue && averageFuelConsumption.Value < 0)
        {
            errors.Add("Average fuel consumption cannot be negative.");
        }

        if(errors.Count > 0)
        {
            throw new ValidationException(string.Join("; ", errors));
        }
    }
}