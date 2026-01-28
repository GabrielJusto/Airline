
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Airline.DTO;
using Airline.RequestBodies;

namespace Airline.Models;

public class Route
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RouteID { get; set; }

    public int FromAirportId { get; set; }
    public int ToAirportId { get; set; }
    public double Distance { get; set; }
    public required virtual Airport FromAirport { get; set; }
    public required virtual Airport ToAirport { get; set; }
    public Route() { }

    public Route(RouteInsertRequestBody data)
    {
        Distance = data.Distance;
    }

}
