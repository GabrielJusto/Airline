package com.bonatto.airline.domain.airline.model;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

import com.bonatto.airline.domain.airline.dto.AirlineRegisterData;

import jakarta.persistence.ElementCollection;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import lombok.Getter;

@Entity(name= "Airline")
@Table(name= "airline")
@Getter
public class Airline {

    @Id @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private boolean active;
    private String name;

    @OneToMany(mappedBy = "airline")
    private List<Aircraft> aircraftList;

    @ElementCollection
    private List<String> destinations;
    
    @OneToMany(mappedBy = "airline")
    private List<Flight> flights;


    public Airline()
    {
        this.active = true;
        aircraftList = new LinkedList<>();
        destinations = new LinkedList<>();
        flights = new ArrayList<>();
    }

    public Airline(AirlineRegisterData data)
    {
        this.active = true;
        this.name = data.name();
        aircraftList = new LinkedList<>();
        destinations = new LinkedList<>();
        flights = new ArrayList<>();
    }

    public void deactivate() {
        this.active = false;
    }
}
