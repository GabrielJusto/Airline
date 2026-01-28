using Airline.Models;

namespace Airline.DTO.AirportDTOs;

public record AirportListDetailDTO(
    int AirportId,
    string IATACode,
    string Name,
    string City,
    string Country
)
{
    public AirportListDetailDTO(Airport airport) : this(
        airport.AirportId,
        airport.IATACode,
        airport.Name,
        airport.City,
        airport.Country
    )
    { }
}