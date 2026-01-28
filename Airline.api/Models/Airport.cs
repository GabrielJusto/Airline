using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airline.Models;

public class Airport
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AirportId { get; set; }
    public string IATACode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}