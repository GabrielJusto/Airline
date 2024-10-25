﻿
using AirlineAPI.DTO;

namespace AirlineAPIV2.Models
{
    public class Aircraft
    {
        public int AircraftID {  get; set; }
        public virtual ICollection<Route> Routes { get; set; } = [];
        public string Model { get; set; } = string.Empty;
        public int Capacity { get; set; } 
        public double Range { get; set; }  

         public Aircraft(){}
        public Aircraft(AircraftCreateDTO createData)
        {
            this.Model = createData.Model;
            this.Capacity = createData.Capacity;
            this.Range = createData.Range;
        }

        public void Update(AircraftUpdateDto updateData)
        {
            if(updateData.Capacity != this.Capacity)
            {
                this.Capacity = updateData.Capacity;
            }
            if(updateData.Range != this.Range)
            {
                this.Range = updateData.Range;
            }
        }

    }
}
