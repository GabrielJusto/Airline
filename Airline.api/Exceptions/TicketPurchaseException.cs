
using Airline.Observability;

namespace Airline.Exceptions;

public class TicketPurchaseException : AirlineException
{
    public int UserId { get; }
    public int FlightId { get; }

    public TicketPurchaseException(int userId, int flightId)
        : base($"User {userId} already has a ticket for flight {flightId}.",
        new Dictionary<string, object?>
        {
            [LogAttributeNames.UserId] = userId,
            [LogAttributeNames.FlightId] = flightId
        })
    {
    }
}