using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlineAPIV2.Models;

namespace AirlineAPI.Dataviews
{
    public class AircraftDetailView(Aircraft aircraft)
    {
        public int AircraftID { get; } = aircraft.AircraftID;
        public string? Model { get; } = aircraft.Model;
        public int Capacity { get; } = aircraft.Capacity;
        public double Range { get; } = aircraft.Range;
    }
}