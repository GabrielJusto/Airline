using Airline.Models;

namespace Airline.DTO;

public class FlightDetailDTO(Flight flight)
{
    public int FlightId { get; set; } = flight.FlightId;
    public DateTime Departure { get; set; } = flight.Departure.UtcDateTime;
    public DateTime Arrival { get; set; } = flight.Arrival.UtcDateTime;
    public string AircraftModel { get; set; } = flight.Aircraft.Model;
}