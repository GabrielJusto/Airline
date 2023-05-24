package com.bonatto.airline.domain.airline.dto;

import com.bonatto.airline.domain.airline.model.Airline;

public record AirlineData(String name) {

    public AirlineData(Airline airline)
    {
        this(airline.getName());
    }
}
