
namespace Airline.DTO;

public class SeatListFilterDTO
{
    public int? FlightId { get; set; }
    public string? FromIATACode { get; set; }
    public string? ToIATACode { get; set; }
    public DateTimeOffset? DepartureDate { get; set; }
}