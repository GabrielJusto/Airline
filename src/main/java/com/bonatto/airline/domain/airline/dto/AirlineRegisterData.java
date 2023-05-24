package com.bonatto.airline.domain.airline.dto;

import jakarta.validation.constraints.NotBlank;

public record AirlineRegisterData(@NotBlank String name) {
}
