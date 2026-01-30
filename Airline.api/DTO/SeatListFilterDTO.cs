
namespace Airline.DTO;

public class SeatListFilterDTO
{
    public int? FlightId { get; set; }
    public int? RouteId { get; set; }
    public DateOnly? DepartureDate { get; set; }
}