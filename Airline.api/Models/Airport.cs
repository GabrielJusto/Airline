using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Airline.DTO;
using Airline.DTO.AirportDTOs;

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

    public Airport() { }

    public Airport(AirportCreateDTO data)
    {
        IATACode = data.IATACode;
        Name = data.Name;
        City = data.City;
        Country = data.Country;
    }
}