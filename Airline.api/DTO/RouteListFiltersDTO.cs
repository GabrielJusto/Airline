namespace Airline.DTO;


public record RouteListFiltersDTO
{
    public int? FromAirportId { get; init; }
    public int? ToAirportId { get; init; }
}
