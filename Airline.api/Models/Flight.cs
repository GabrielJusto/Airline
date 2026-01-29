using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Airline.DTO;

namespace Airline.Models;

public class Flight
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FlightId { get; set; }
    public required virtual Route Route { get; set; }
    public required virtual Aircraft Aircraft { get; set; }
    public int RouteId { get; set; }
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public Flight() { }

    public Flight(FlightCreateDTO data)
    {
        Departure = data.Departure.ToUniversalTime();
        Arrival = data.Arrival.ToUniversalTime();
    }
}