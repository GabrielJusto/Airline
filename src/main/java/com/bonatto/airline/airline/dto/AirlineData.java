package com.bonatto.airline.airline.dto;

import com.bonatto.airline.airline.model.Airline;

public record AirlineData(String name) {

    public AirlineData(Airline airline)
    {
        this(airline.getName());
    }
}
