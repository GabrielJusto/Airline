package com.bonatto.airline.domain.aircraft.dto;

import java.util.List;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

public record AircraftRegisterData(
        @NotNull
        Long airlineId,
        @NotBlank
        String model,
        @NotNull
        int passengerCapacity,
        @NotNull
        double takeoffWeight,
        @NotNull
        double aircraftRange,
        @NotNull
        List<SeatRegisterData> seats
) {
}
