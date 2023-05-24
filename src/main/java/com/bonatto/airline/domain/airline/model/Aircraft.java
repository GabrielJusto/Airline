package com.bonatto.airline.domain.airline.model;

import com.bonatto.airline.domain.airline.dto.AircraftRegisterData;
import com.bonatto.airline.domain.airline.repository.AirlineRepository;
import jakarta.persistence.*;
import lombok.NoArgsConstructor;

import java.util.Optional;

@Entity(name="Aircraft")
@Table(name= "aircraft")
@NoArgsConstructor
public class Aircraft {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @ManyToOne
    private Airline airline;

    private String model;

    private int passengerCapacity;

    private double takeoffWeight;

    private double aircraftRange;

    @Embedded
    private Scheduler scheduler;


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



    }
}
