using Airline.Models;

namespace Airline.DTO;

public class AircraftDetailDTO
{
    public int AircraftID { get; set; }
    public string Model { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public double Range { get; set; }
    public double AverageFuelConsumption { get; set; }

    public AircraftDetailDTO(Aircraft aircraft)
    {
        AircraftID = aircraft.AircraftID;
        Model = aircraft.Model;
        Capacity = aircraft.Capacity;
        Range = aircraft.Range;
        AverageFuelConsumption = aircraft.AverageFuelConsumption;
    }
}