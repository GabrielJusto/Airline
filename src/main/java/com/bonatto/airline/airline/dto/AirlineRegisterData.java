package com.bonatto.airline.airline.dto;

import jakarta.validation.constraints.NotBlank;

public record AirlineRegisterData(@NotBlank String name) {
}
