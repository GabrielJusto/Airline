package com.bonatto.airline.airline.model;

import com.bonatto.airline.airline.dto.AirlineRegisterData;
import jakarta.persistence.*;

import java.util.LinkedList;
import java.util.List;

@Entity
public class Airline {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String name;

    @OneToMany(mappedBy = "airline")
    private List<Aircraft> aircraftList;

    @ElementCollection
    private List<String> destinations;


    public Airline()
    {
        aircraftList = new LinkedList<>();
        destinations = new LinkedList<>();
    }

    public Airline(AirlineRegisterData data)
    {
        this.name = data.name();
        aircraftList = new LinkedList<>();
        destinations = new LinkedList<>();
    }

}
