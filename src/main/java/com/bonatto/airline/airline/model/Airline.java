package com.bonatto.airline.airline.model;

import com.bonatto.airline.airline.dto.AirlineRegisterData;
import jakarta.persistence.*;
import lombok.Getter;
import java.util.LinkedList;
import java.util.List;

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


    public Airline()
    {
        this.active = true;
        aircraftList = new LinkedList<>();
        destinations = new LinkedList<>();
    }

    public Airline(AirlineRegisterData data)
    {
        this.active = true;
        this.name = data.name();
        aircraftList = new LinkedList<>();
        destinations = new LinkedList<>();
    }

    public void deactivate() {
        this.active = false;
    }
}
