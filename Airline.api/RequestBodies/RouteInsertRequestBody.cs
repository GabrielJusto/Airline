namespace Airline.RequestBodies;

public record RouteInsertRequestBody(
    int FromAirportId,
    int ToAirportId,
    double Distance
);

