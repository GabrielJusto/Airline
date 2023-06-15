package com.bonatto.airline.domain.aircraft.dto;

public record AircraftUpdateData(

        Long id,
        Long airlineId,
        int passengerCapacity

) {

}
