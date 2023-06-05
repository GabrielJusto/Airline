package com.bonatto.airline.domain.airline.dto;

public record AircraftUpdateData(

        Long id,
        Long airlineId,
        int passengerCapacity

) {

}
