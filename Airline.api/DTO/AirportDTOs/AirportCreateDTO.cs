namespace Airline.DTO.AirportDTOs;

public record AirportCreateDTO
(
    string IATACode,
    string Name,
    string City,
    string Country,
    string TimeZoneName
)
{

}