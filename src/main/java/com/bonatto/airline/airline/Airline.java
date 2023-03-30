package com.bonatto.airline.airline;

import jakarta.persistence.*;
import jakarta.validation.constraints.NotBlank;

import java.util.LinkedList;
import java.util.List;

@Entity
public class Airline {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @NotBlank
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

    public Airline(AirlineRegisterDto data)
    {
        this.name = data.name();
        aircraftList = new LinkedList<>();
        destinations = new LinkedList<>();
    }

}
