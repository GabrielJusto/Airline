
using Airline.Models;

namespace Airline.DTO.SeatDTOs;

public record SeatTicketListDTO
(
    int SeatId,
    string FromIATACode,
    string FromCity,
    string ToIATACode,
    string ToCity,
    DateTimeOffset Departure,
    DateTimeOffset Arrival,
    string FlightDuration,
    decimal Price,
    string FlightNumber,
    string SeatClass
)
{
    public SeatTicketListDTO(Seat seat) : this(
        seat.SeatId,
        seat.Flight.Route.FromAirport.IATACode,
        seat.Flight.Route.FromAirport.City,
        seat.Flight.Route.ToAirport.IATACode,
        seat.Flight.Route.ToAirport.City,
        seat.Flight.Departure,
        seat.Flight.Arrival,
        (seat.Flight.Arrival - seat.Flight.Departure).ToString(@"hh\:mm"),
        seat.Price,
        seat.Flight.FlightId.ToString("F4"),
        seat.SeatClass.ToString()
     )
    { }
}