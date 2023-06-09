package com.bonatto.airline.domain.airline.model;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

import com.bonatto.airline.domain.airline.dto.AircraftRegisterData;
import com.bonatto.airline.domain.airline.dto.AircraftUpdateData;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;

import jakarta.persistence.Entity;
import jakarta.persistence.EntityNotFoundException;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Entity(name="Aircraft")
@Table(name= "aircraft")
@NoArgsConstructor
@AllArgsConstructor
@Getter
public class Aircraft {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    private Airline airline;

    private String model;

    private int passengerCapacity;

    private double takeoffWeight;

    private double aircraftRange;
    
    @OneToMany(mappedBy = "aircraft")
    private List<Flight> flights;



    public Aircraft(AircraftRegisterData data, AirlineRepository airlineRepo)
    {
        Optional<Airline> airlineOp = airlineRepo.findById(data.airlineId());

        if(airlineOp.isEmpty())
            return;

        Airline airline = airlineOp.get();

        this.airline = airline;
        this.model = data.model();
        this.passengerCapacity = data.passengerCapacity();
        this.takeoffWeight = data.takeoffWeight();
        this.aircraftRange = data.aircraftRange();
        
        this.flights = new ArrayList<>();
    }


    public void update(AircraftUpdateData data, AirlineRepository repo)
    {
        if(data.airlineId() != null && data.airlineId() != this.airline.getId())
        {

            Optional<Airline> airlineOp = repo.findById(data.airlineId());
            if(airlineOp.isEmpty())
                throw new EntityNotFoundException();


            this.airline = airlineOp.get();
        }

        if(data.passengerCapacity() != 0 && data.passengerCapacity() != this.passengerCapacity)
            this.passengerCapacity = data.passengerCapacity();
    }

}
