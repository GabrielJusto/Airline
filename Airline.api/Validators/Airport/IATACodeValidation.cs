
using System.ComponentModel.DataAnnotations;

using Airline.DTO;
using Airline.DTO.AirportDTOs;

namespace Airline.Validators.Airport;

public class IATACodeValidation(
    AirportCreateDTO data
) : IValidation
{

    public readonly AirportCreateDTO _data = data;
    public void Validate()
    {
        if(_data.IATACode.Length != 3)
        {
            throw new ValidationException("IATA Code must has 3 characteres.");
        }
        if(!_data.IATACode.ToUpper().Equals(_data.IATACode))
        {
            throw new ValidationException("IATA Code must be all uppercase.");
        }
        if(!_data.IATACode.All(char.IsLetter))
        {
            throw new ValidationException("IATA Code must contain only letters.");
        }
    }
}